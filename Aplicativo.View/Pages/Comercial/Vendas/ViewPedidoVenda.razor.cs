using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Comercial.Vendas
{
    public partial class ViewPedidoVendaPage : ComponentBase
    {

        public PedidoVenda ViewModel = new PedidoVenda();

        [Parameter] public ListItemViewLayout<PedidoVenda> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements

        public ViewPedidoVendaAndamento ViewPedidoVendaAndamento { get; set; }

        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public ViewPesquisa<Pessoa> ViewPesquisaCliente { get; set; }
        public TextBox TxtCNPJ { get; set; }
        public TextBox TxtTelefone { get; set; }
        public DropDownList DplStatus { get; set; }

        public ViewPesquisa<Pessoa> ViewPesquisaVendedor { get; set; }
        public DatePicker DtpData { get; set; }
        public DatePicker DtpExpedicao { get; set; }

        public ViewPesquisa<Pessoa> ViewPesquisaTransportadora { get; set; }

        public ViewPedidoVendaItem ViewPedidoVendaItem { get; set; }
        public ViewPedidoVendaPagamento ViewPedidoVendaPagamento { get; set; }


        public SfGrid<PedidoVendaAndamento> GridViewAndamento { get; set; }
        public List<PedidoVendaAndamento> ListAndamento { get; set; } = new List<PedidoVendaAndamento>();

        #endregion

        private void InitializeComponents()
        {


            ViewPesquisaCliente.Where.Clear();
            ViewPesquisaVendedor.Where.Clear();
            ViewPesquisaTransportadora.Where.Clear();

            ViewPesquisaCliente.AddInclude("PessoaContato");
            ViewPesquisaCliente.AddInclude("PessoaContato.Contato");
            ViewPesquisaCliente.AddWhere("IsCliente == @0", true);
            ViewPesquisaCliente.AddWhere("Ativo == @0", true);

            ViewPesquisaVendedor.AddWhere("IsFuncionario == @0", true);
            ViewPesquisaVendedor.AddWhere("Ativo == @0", true);

            ViewPesquisaTransportadora.AddWhere("IsTransportadora == @0", true);
            ViewPesquisaTransportadora.AddWhere("Ativo == @0", true);

            DplStatus.LoadDropDownList("PedidoVendaStatusID", "Descricao", new DropDownListItem(null, "[Selecione]"), 
                HelpParametros.Parametros.PedidoVendaStatus.Where(c => 
                    (c.IsFinalizado ?? false) == false && 
                    (c.IsSeparado ?? false) == false && 
                    (c.IsConferido ?? false) == false && 
                    (c.IsFaturado ?? false) == false &&
                    (c.IsEntregue ?? false) == false
                ).ToList()
            );


        }

        protected async Task Page_Load(object args)
        {

            InitializeComponents();

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<PedidoVenda>();

            Query.AddInclude("Vendedor");
            Query.AddInclude("Cliente");
            Query.AddInclude("Cliente.PessoaContato");
            Query.AddInclude("Cliente.PessoaContato.Contato");
            Query.AddInclude("PedidoVendaItem");
            Query.AddInclude("PedidoVendaItem.Operacao");
            Query.AddInclude("PedidoVendaItem.Produto");
            Query.AddInclude("PedidoVendaPagamento");
            Query.AddInclude("PedidoVendaPagamento.Titulo");
            Query.AddInclude("PedidoVendaPagamento.Titulo.TituloDetalhe");
            Query.AddInclude("PedidoVendaPagamento.Titulo.TituloDetalhe.PlanoConta");
            Query.AddInclude("PedidoVendaPagamento.Titulo.TituloDetalhe.CentroCusto");
            Query.AddInclude("PedidoVendaPagamento.Titulo.TituloDetalhe.ContaBancaria");
            Query.AddInclude("PedidoVendaPagamento.Titulo.TituloDetalhe.FormaPagamento");

            Query.AddInclude("PedidoVendaAndamento");
            Query.AddInclude("PedidoVendaAndamento.PedidoVendaStatus");

            Query.AddInclude("Transportadora");
            Query.AddWhere("PedidoVendaID == @0", ((PedidoVenda)args).PedidoVendaID);
            
            ViewModel = await Query.FirstOrDefault();

            EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;

            TxtCodigo.Text = ViewModel.PedidoVendaID.ToStringOrNull();
            ViewPesquisaCliente.Value = ViewModel.ClienteID.ToStringOrNull();
            ViewPesquisaCliente.Text = ViewModel.Cliente?.NomeFantasia.ToStringOrNull();
            TxtCNPJ.Text = ViewModel.Cliente?.CNPJ_Formatado.ToStringOrNull();
            TxtTelefone.Text = ViewModel.Cliente?.PessoaContato?.FirstOrDefault()?.Contato?.Telefone;
            DplStatus.SelectedValue = ViewModel.PedidoVendaStatusID.ToStringOrNull();

            ViewPesquisaVendedor.Value = ViewModel.VendedorID.ToStringOrNull();
            ViewPesquisaVendedor.Text = ViewModel.Vendedor?.NomeFantasia.ToStringOrNull();

            DtpData.Value = ViewModel.Data;
            DtpExpedicao.Value = ViewModel.Expedicao;

            ViewPesquisaTransportadora.Value = ViewModel.TransportadoraID.ToStringOrNull();
            ViewPesquisaTransportadora.Text = ViewModel.Transportadora?.NomeFantasia.ToStringOrNull();

            ViewPedidoVendaItem.ListView.Items = ViewModel.PedidoVendaItem.ToList();

            if (ViewModel.PedidoVendaPagamento?.FirstOrDefault()?.Titulo?.TituloDetalhe != null)
            {
                ViewPedidoVendaPagamento.ListView.Items = ViewModel.PedidoVendaPagamento?.FirstOrDefault()?.Titulo?.TituloDetalhe?.ToList();
            }


            ListAndamento.Clear();
            ListAndamento.AddRange(ViewModel.PedidoVendaAndamento.ToList());
            GridViewAndamento.Refresh();

            ViewPedidoVendaItem_Save();

            ViewPedidoVendaItem.CalcularTotais();


        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.ItemViewMode = ItemViewMode.New;

            EditItemViewLayout.LimparCampos(this);

            ViewPesquisaCliente.Clear();

            ViewPesquisaVendedor.Value = HelpParametros.Parametros.UsuarioLogado?.FuncionarioID.ToStringOrNull();
            ViewPesquisaVendedor.Text = HelpParametros.Parametros.UsuarioLogado?.Funcionario?.NomeFantasia.ToStringOrNull();


            DtpData.Value = DateTime.Now;
            DtpExpedicao.Value = DateTime.Now;

            ViewPesquisaTransportadora.Clear();

            ViewPedidoVendaItem.ListView.Items = new List<PedidoVendaItem>();

            ViewPedidoVendaPagamento.vTotal_Items = 0;
            ViewPedidoVendaPagamento.vTotal_Pagamento = 0;
            ViewPedidoVendaPagamento.ListView.Items = new List<TituloDetalhe>();

            ViewModel.PedidoVendaPagamento.Clear();

            ListAndamento.Clear();
            GridViewAndamento.Refresh();

            ViewPedidoVendaItem_Save();

            ViewPedidoVendaItem.CalcularTotais();

            await TabSet.Active("Principal");

            ViewPesquisaCliente.Focus();

        }

        protected async Task BtnSalvar_Click()
        {

            await Validar();

            await Salvar();

            await App.JSRuntime.InvokeVoidAsync("alert", "Salvo com sucesso!!");

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;
                await Page_Load(ViewModel);
            }
            else
            {
                await EditItemViewLayout.ViewModal.Hide();
            }

        }

        private async Task Validar()
        {

            if (ViewPesquisaCliente.Value.ToIntOrNull() == null)
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe o cliente!", ViewPesquisaCliente.Element);
            }

            if (DplStatus.SelectedValue == null)
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe o status!", DplStatus.Element);
            }

            if (ViewPesquisaVendedor.Value.ToIntOrNull() == null)
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe o vendedor!", DplStatus.Element);
            }

            if (ViewPedidoVendaItem.ListView.Items.Count == 0)
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Nenhum item informado para venda!");
            }



        }

        private async Task Salvar()
        {

            ViewModel.PedidoVendaID = TxtCodigo.Text.ToIntOrNull();

            ViewModel.ClienteID = ViewPesquisaCliente.Value.ToIntOrNull();
            ViewModel.Cliente = null;

            ViewModel.VendedorID = ViewPesquisaVendedor.Value.ToIntOrNull();
            ViewModel.Vendedor = null;

            ViewModel.Data = DtpData.Value;
            ViewModel.Expedicao = DtpExpedicao.Value;

            ViewModel.PedidoVendaStatusID = DplStatus.SelectedValue.ToIntOrNull();

            ViewModel.TransportadoraID = ViewPesquisaTransportadora.Value.ToIntOrNull();
            ViewModel.Transportadora = null;

            ViewModel.PedidoVendaItem = ViewPedidoVendaItem.ListView.Items.ToList();

            foreach (var item in ViewModel.PedidoVendaItem)
            {
                item.Operacao = null;
                item.Produto = null;
            }


            var Query = new HelpQuery<PedidoVenda>();

            ViewModel.PedidoVendaAndamento.Clear();

            if (ViewModel.PedidoVendaID == null)
            {
                ViewModel.PedidoVendaAndamento.Add(new PedidoVendaAndamento()
                {
                    UsuarioID = HelpParametros.Parametros.UsuarioLogado.UsuarioID,
                    Data = DateTime.Now,
                    PedidoVendaStatusID = DplStatus.SelectedValue.ToIntOrNull(),
                    Observacao = "Criado",
                });
            }

            if (ViewModel.PedidoVendaPagamento.Count == 0)
            {
                ViewModel.PedidoVendaPagamento = new List<PedidoVendaPagamento>();
                ViewModel.PedidoVendaPagamento.Add(new PedidoVendaPagamento() { Titulo = new Titulo() { DataLancamento = DateTime.Now, TituloDetalhe = ViewPedidoVendaPagamento.ListView.Items.ToList() } });
            }
            else
            {
                ViewModel.PedidoVendaPagamento.FirstOrDefault().Titulo.TituloDetalhe = ViewPedidoVendaPagamento.ListView.Items.ToList();
            }

            foreach (var item in ViewModel.PedidoVendaPagamento)
            {

                item.Titulo.Ativo = false;

                foreach (var item2 in item.Titulo.TituloDetalhe)
                {
                    item2.PessoaID = ViewModel.ClienteID;
                    item2.Pessoa = null;
                    item2.ContaBancaria = null;
                    item2.FormaPagamento = null;
                    item2.PlanoConta = null;
                    item2.CentroCusto = null;
                }
            }

            var HelpUpdate = new HelpUpdate();

            HelpUpdate.Add(ViewModel);

            var Changes = await HelpUpdate.SaveChanges();


            ViewModel = HelpUpdate.Bind<PedidoVenda>(Changes[0]);

        }

        protected async Task BtnFinalizar_Click()
        {
            try
            {

                await Validar();

                if (DplStatus.SelectedValue == null)
                {
                    await TabSet.Active("Principal");
                    throw new EmptyException("Informe o status!", DplStatus.Element);
                }

                await Salvar();

                await Page_Load(ViewModel);

                await ListView.ListViewBtnPesquisa.BtnPesquisar_Click();

                ViewPedidoVendaAndamento.IsFinalizado = true;
                ViewPedidoVendaAndamento.PedidoVendaID = new List<int?>() { TxtCodigo.Text.ToIntOrNull() };

                await ViewPedidoVendaAndamento.EditItemViewLayout.Show(null);


            }
            catch (EmptyException)
            {

            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        protected async Task ViewPedidoVendaAndamento_Confirm()
        {

            await App.JSRuntime.InvokeVoidAsync("alert", "Finalizado com sucesso!!");

            await ViewPedidoVendaAndamento.EditItemViewLayout.ViewModal.Hide();
            await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPedidoVendaAndamento_Finally()
        {

            await ListView.ListViewBtnPesquisa.BtnPesquisar_Click();

        }

        protected async Task BtnExcluir_Click()
        {

            await Excluir(new List<int> { TxtCodigo.Text.ToInt() });

            await EditItemViewLayout.ViewModal.Hide();

        }

        public async Task Excluir(List<int> args)
        {

            var Query = new HelpQuery<PedidoVenda>();

            Query.AddWhere("PedidoVendaID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            foreach (var item in ViewModel)
            {
                //item.Ativo = false;
            }

            //await Query.Update(ViewModel, false);

        }

        protected void ViewPesquisaCliente_Change(object args)
        {

            if (args == null)
            {
                TxtCNPJ.Text = null;
                TxtTelefone.Text = null;
                return;
            }

            var Pessoa = (Pessoa)args;

            string Mask;

            if (Pessoa.TipoPessoaID == TipoPessoa.Fisica)
            {
                TxtCNPJ.Label = "CPF";
                Mask = "###.###.###-##";
            }
            else
            {
                TxtCNPJ.Label = "CNPJ";
                Mask = "##.###.###/####-##";
            }

            TxtCNPJ.Text = Pessoa.CNPJ?.StringFormat(Mask);
            TxtTelefone.Text = Pessoa.PessoaContato?.FirstOrDefault()?.Contato?.Telefone;

        }

        protected decimal vTotal_Items { get; set; } = 0;

        protected void ViewPedidoVendaItem_Save()
        {

            vTotal_Items = ViewPedidoVendaItem.ListView.Items.Sum(c => c.vTotal ?? 0);
            
            ViewPedidoVendaPagamento.vTotal_Items = vTotal_Items;
            ViewPedidoVendaPagamento.CalculaPagamento();

        }

    }
}