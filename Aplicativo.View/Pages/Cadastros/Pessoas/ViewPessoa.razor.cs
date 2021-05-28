using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Pessoas
{
    public partial class ViewPessoaPage : ComponentBase
    {

        public Pessoa ViewModel = new Pessoa();

        [Parameter] public string Title { get; set; }
        [Parameter] public Tipo Tipo { get; set; }

        [Parameter] public ListItemViewLayout<Pessoa> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements

        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public DropDownList DplTipo { get; set; }
        public TextBox TxtCNPJ { get; set; }
        public TextBox TxtRazaoSocial { get; set; }
        public TextBox TxtNomeFantasia { get; set; }

        public CheckBox ChkCliente { get; set; }
        public CheckBox ChkFornecedor { get; set; }
        public CheckBox ChkTransportadora { get; set; }
        public CheckBox ChkFuncionario { get; set; }


        public TextBox TxtInscricaoEstadual { get; set; }
        public TextBox TxtInscricaoMunicipal { get; set; }
        public DropDownList DplSexo { get; set; }
        public DatePicker DtpAbertura { get; set; }


        public ViewPessoaVendedor ViewPessoaVendedor { get; set; }

        public ViewPessoaEndereco ViewPessoaEndereco { get; set; }

        public ViewPessoaContato ViewPessoaContato { get; set; }

        #endregion

        protected void InitializeComponents()
        {

            DplTipo.Items.Clear();
            DplTipo.Add(((int)TipoPessoa.Fisica).ToString(), "Fisíca");
            DplTipo.Add(((int)TipoPessoa.Juridica).ToString(), "Jurídica");

            DplSexo.Items.Clear();
            DplSexo.Add(null, "[Selecione]");
            DplSexo.Add(((int)Sexo.Masculino).ToString(), "Masculino");
            DplSexo.Add(((int)Sexo.Feminino).ToString(), "Feminino");

        }

        protected async Task Page_Load(object args)
        {

            InitializeComponents();

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<Pessoa>();

            Query.AddInclude("PessoaEndereco");
            Query.AddInclude("PessoaEndereco.Endereco");

            Query.AddInclude("PessoaContato");
            Query.AddInclude("PessoaContato.Contato");

            Query.AddWhere("PessoaID == @0", ((Pessoa)args)?.PessoaID);
            Query.AddTake(1);

            ViewModel = await Query.FirstOrDefault();


            TxtCodigo.Text = ViewModel.PessoaID.ToStringOrNull();
            DplTipo.SelectedValue = ((int?)ViewModel.TipoPessoaID).ToStringOrNull();
            TxtRazaoSocial.Text = ViewModel.RazaoSocial.ToStringOrNull();
            TxtNomeFantasia.Text = ViewModel.NomeFantasia.ToStringOrNull();
            ChkCliente.Checked = ViewModel.IsCliente.ToBoolean();
            ChkFornecedor.Checked = ViewModel.IsFornecedor.ToBoolean();
            ChkTransportadora.Checked = ViewModel.IsTransportadora.ToBoolean();
            ChkFuncionario.Checked = ViewModel.IsFuncionario.ToBoolean();


            TxtInscricaoEstadual.Text = ViewModel.IE.ToStringOrNull();
            TxtInscricaoMunicipal.Text = ViewModel.IM.ToStringOrNull();
            DplSexo.SelectedValue = ((int?)ViewModel.Sexo).ToStringOrNull();
            DtpAbertura.Value = ViewModel.Abertura;


            ViewPessoaVendedor.ListView.Items = ViewModel.Vendedor.ToList();
            ViewPessoaEndereco.ListView.Items = ViewModel.PessoaEndereco.ToList();
            ViewPessoaContato.ListView.Items = ViewModel.PessoaContato.ToList();

        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.LimparCampos(this);

            DplTipo.SelectedValue = ((int)TipoPessoa.Juridica).ToString();

            switch (Tipo)
            {
                case Tipo.Cliente:
                    ChkCliente.Checked = true;
                    break;
                case Tipo.Fornecedor:
                    ChkFornecedor.Checked = true;
                    break;
                case Tipo.Transportadora:
                    ChkTransportadora.Checked = true;
                    break;
                case Tipo.Funcionario:
                    ChkFuncionario.Checked = true;
                    break;
            }

            ViewPessoaVendedor.ListView.Items = new List<PessoaVendedor>();
            ViewPessoaEndereco.ListView.Items = new List<PessoaEndereco>();
            ViewPessoaContato.ListView.Items = new List<PessoaContato>();

            await TabSet.Active("Principal");

            TxtCNPJ.Focus();


        }

        protected void DplTipo_Change(ChangeEventArgs args)
        {

            if ((TipoPessoa)args?.Value.ToIntOrNull() == TipoPessoa.Fisica)
            {
                TxtCNPJ.Label = "CPF";
                TxtCNPJ.Mask = "999.999.999-99";
                TxtRazaoSocial.Label = "Nome completo";
                TxtNomeFantasia.Label = "Apelido";
                TxtInscricaoEstadual.Label = "RG";
                DtpAbertura.Label = "Data nascimento";
            }
            else
            {
                TxtCNPJ.Label = "CNPJ";
                TxtCNPJ.Mask = "99.999.999/9999-99";
                TxtRazaoSocial.Label = "Razão social";
                TxtNomeFantasia.Label = "Nome fantasia";
                TxtInscricaoEstadual.Label = "Insc. Estadual";
                DtpAbertura.Label = "Data abertura";
            }

            TxtCNPJ.Text = null;
            TxtCNPJ.Focus();

        }

        protected async Task BtnSalvar_Click()
        {

            if (string.IsNullOrEmpty(TxtRazaoSocial.Text))
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe a razão social!", TxtRazaoSocial.Element);
            }

            ViewModel.PessoaID = TxtCodigo.Text.ToIntOrNull();
            ViewModel.TipoPessoaID = (TipoPessoa?)DplTipo.SelectedValue.ToIntOrNull();
            ViewModel.RazaoSocial = TxtRazaoSocial.Text.ToStringOrNull();
            ViewModel.NomeFantasia = TxtNomeFantasia.Text.ToStringOrNull();
            ViewModel.IsCliente = ChkCliente.Checked;
            ViewModel.IsFornecedor = ChkFornecedor.Checked;
            ViewModel.IsTransportadora = ChkTransportadora.Checked;
            ViewModel.IsFuncionario = ChkFuncionario.Checked;

            ViewModel.IE = TxtInscricaoEstadual.Text.ToStringOrNull();
            ViewModel.IM = TxtInscricaoMunicipal.Text.ToStringOrNull();
            ViewModel.Sexo = (Sexo?)DplSexo.SelectedValue.ToIntOrNull();
            ViewModel.Abertura = DtpAbertura.Value;

            ViewModel.Vendedor = ViewPessoaVendedor.ListView.Items.ToList();
            ViewModel.PessoaEndereco = ViewPessoaEndereco.ListView.Items.ToList();
            ViewModel.PessoaContato = ViewPessoaContato.ListView.Items.ToList();


            var Query = new HelpQuery<Pessoa>();

            ViewModel = await Query.Update(ViewModel);

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;
                TxtCodigo.Text = ViewModel.PessoaID.ToStringOrNull();
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

            var Query = new HelpQuery<Pessoa>();

            Query.AddWhere("PessoaID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            foreach (var item in ViewModel)
            {
                item.Ativo = false;
            }

            await Query.Update(ViewModel, false);

        }
    }
}