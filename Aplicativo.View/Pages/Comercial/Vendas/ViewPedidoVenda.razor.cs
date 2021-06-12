using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public ViewPesquisa<Pessoa> ViewPesquisaCliente { get; set; }
        public TextBox TxtCNPJ { get; set; }
        public TextBox TxtTelefone { get; set; }

        public ViewPesquisa<Pessoa> ViewPesquisaVendedor { get; set; }
        public DatePicker DtpData { get; set; }
        public DatePicker DtpExpedicao { get; set; }

        public ViewPesquisa<Pessoa> ViewPesquisaTransportadora { get; set; }

        public ViewPedidoVendaItem ViewPedidoVendaItem { get; set; }
        public ViewPedidoVendaPagamento ViewPedidoVendaPagamento { get; set; }

        #endregion

        protected async Task Page_Load(object args)
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

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<PedidoVenda>();

            Query.AddInclude("Vendedor");
            Query.AddInclude("Cliente");
            Query.AddInclude("Cliente.PessoaContato");
            Query.AddInclude("Cliente.PessoaContato.Contato");
            Query.AddInclude("PedidoVendaItem");
            Query.AddInclude("PedidoVendaItem.Produto");
            Query.AddInclude("PedidoVendaPagamento");
            Query.AddInclude("PedidoVendaPagamento.Titulo");
            Query.AddInclude("PedidoVendaPagamento.Titulo.TituloDetalhe");
            Query.AddInclude("PedidoVendaPagamento.Titulo.TituloDetalhe.PlanoConta");
            Query.AddInclude("PedidoVendaPagamento.Titulo.TituloDetalhe.CentroCusto");
            Query.AddInclude("PedidoVendaPagamento.Titulo.TituloDetalhe.ContaBancaria");
            Query.AddInclude("PedidoVendaPagamento.Titulo.TituloDetalhe.FormaPagamento");
            Query.AddInclude("Transportadora");
            Query.AddWhere("PedidoVendaID == @0", ((PedidoVenda)args).PedidoVendaID);
            
            ViewModel = await Query.FirstOrDefault();

            EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;

            TxtCodigo.Text = ViewModel.PedidoVendaID.ToStringOrNull();
            ViewPesquisaCliente.Value = ViewModel.ClienteID.ToStringOrNull();
            ViewPesquisaCliente.Text = ViewModel.Cliente?.NomeFantasia.ToStringOrNull();
            TxtCNPJ.Text = ViewModel.Cliente?.CNPJ_Formatado.ToStringOrNull();
            TxtTelefone.Text = ViewModel.Cliente?.PessoaContato?.FirstOrDefault()?.Contato?.Telefone;

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

            ViewPedidoVendaItem_Save();

            ViewPedidoVendaItem.CalcularTotais();

            await TabSet.Active("Principal");

            ViewPesquisaCliente.Focus();

        }

        protected async Task BtnSalvar_Click()
        {

            //if (string.IsNullOrEmpty(TxtLogin.Text))
            //{
            //    await TabSet.Active("Principal");
            //    throw new EmptyException("Informe o login!", TxtLogin.Element);
            //}

            ViewModel.PedidoVendaID = TxtCodigo.Text.ToIntOrNull();

            ViewModel.ClienteID = ViewPesquisaCliente.Value.ToIntOrNull();
            ViewModel.Cliente = null;

            ViewModel.VendedorID = ViewPesquisaVendedor.Value.ToIntOrNull();
            ViewModel.Vendedor = null;

            ViewModel.Data = DtpData.Value;
            ViewModel.Expedicao = DtpExpedicao.Value;

            ViewModel.TransportadoraID = ViewPesquisaTransportadora.Value.ToIntOrNull();
            ViewModel.Transportadora = null;

            ViewModel.PedidoVendaItem = ViewPedidoVendaItem.ListView.Items.ToList();

            foreach(var item in ViewModel.PedidoVendaItem)
            {
                item.Produto = null;
            }
            

            var Query = new HelpQuery<PedidoVenda>();


            if (ViewModel.PedidoVendaID == null || ViewModel.PedidoVendaPagamento.Count == 0 || ViewModel.PedidoVendaPagamento != null)
            {
                ViewModel.PedidoVendaPagamento = new List<PedidoVendaPagamento>();
                ViewModel.PedidoVendaPagamento.Add(new PedidoVendaPagamento() { Titulo = new Titulo() { DataLancamento = DateTime.Now, TituloDetalhe = ViewPedidoVendaPagamento.ListView.Items.ToList() } });
            }
            else
            {
                ViewModel.PedidoVendaPagamento.FirstOrDefault().Titulo.TituloDetalhe = ViewPedidoVendaPagamento.ListView.Items.ToList();
            }

            foreach(var item in ViewModel.PedidoVendaPagamento)
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