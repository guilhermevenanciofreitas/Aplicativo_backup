using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Configuracao.Empresas
{
    public partial class ViewEmpresaPage : ComponentBase
    {

        public Empresa ViewModel = new Empresa();

        [Parameter] public ListItemViewLayout<Empresa> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements

        public TabSet TabSet { get; set; }

        public TextBox TxtCodigo { get; set; }
        public DropDownList DplTipo { get; set; }
        public TextBox TxtCNPJ { get; set; }
        public TextBox TxtRazaoSocial { get; set; }
        public TextBox TxtNomeFantasia { get; set; }

        public TextBox TxtInscricaoEstadual { get; set; }
        public TextBox TxtInscricaoMunicipal { get; set; }
        public DropDownList DplSexo { get; set; }
        public DatePicker DtpAbertura { get; set; }


        public ViewEmpresaEndereco ViewEmpresaEndereco { get; set; }
        public ViewEmpresaCertificado ViewEmpresaCertificado { get; set; }

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

            var Query = new HelpQuery<Empresa>();

            //Query.AddInclude("Vendedores");
            //Query.AddInclude("Vendedores.Vendedor");

            Query.AddInclude("EmpresaEndereco");
            Query.AddInclude("EmpresaEndereco.Endereco");

            Query.AddInclude("EmpresaCertificado");
            Query.AddInclude("EmpresaCertificado.Certificado");
            Query.AddInclude("EmpresaCertificado.Certificado.Arquivo");

            Query.AddWhere("EmpresaID == @0", ((Empresa)args)?.EmpresaID);
            
            ViewModel = await Query.FirstOrDefault();

            EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;

            TxtCodigo.Text = ViewModel.EmpresaID.ToStringOrNull();
            DplTipo.SelectedValue = ((int?)ViewModel.TipoPessoaID).ToStringOrNull();
            TxtCNPJ.Text = ViewModel.CNPJ_Formatado.ToStringOrNull();
            TxtRazaoSocial.Text = ViewModel.RazaoSocial.ToStringOrNull();
            TxtNomeFantasia.Text = ViewModel.NomeFantasia.ToStringOrNull();
            //ChkCliente.Checked = ViewModel.IsCliente.ToBoolean();
            //ChkFornecedor.Checked = ViewModel.IsFornecedor.ToBoolean();
            //ChkTransportadora.Checked = ViewModel.IsTransportadora.ToBoolean();
            //ChkFuncionario.Checked = ViewModel.IsFuncionario.ToBoolean();


            TxtInscricaoEstadual.Text = ViewModel.IE.ToStringOrNull();
            //TxtInscricaoMunicipal.Text = ViewModel.IM.ToStringOrNull();
            //DplSexo.SelectedValue = ((int?)ViewModel.Sexo).ToStringOrNull();
            //DtpAbertura.Value = ViewModel.Abertura;


            ViewEmpresaEndereco.ListView.Items = ViewModel.EmpresaEndereco.ToList();
            ViewEmpresaCertificado.ListView.Items = ViewModel.EmpresaCertificado.ToList();

        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.ItemViewMode = ItemViewMode.New;

            EditItemViewLayout.LimparCampos(this);

            DplTipo.SelectedValue = ((int)TipoPessoa.Juridica).ToString();


            ViewEmpresaEndereco.ListView.Items = new List<EmpresaEndereco>();
            ViewEmpresaCertificado.ListView.Items = new List<EmpresaCertificado>();

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

            ViewModel.EmpresaID = TxtCodigo.Text.ToIntOrNull();
            //ViewModel.TipoPessoaID = (TipoPessoa?)DplTipo.SelectedValue.ToIntOrNull();
            ViewModel.CNPJ = TxtCNPJ.Text?.Replace("-", "")?.Replace(".","")?.Replace("/", "")?.ToStringOrNull();
            ViewModel.RazaoSocial = TxtRazaoSocial.Text.ToStringOrNull();
            ViewModel.NomeFantasia = TxtNomeFantasia.Text.ToStringOrNull();
            //ViewModel.IsCliente = ChkCliente.Checked;
            //ViewModel.IsFornecedor = ChkFornecedor.Checked;
            //ViewModel.IsTransportadora = ChkTransportadora.Checked;
            //ViewModel.IsFuncionario = ChkFuncionario.Checked;

            ViewModel.IE = TxtInscricaoEstadual.Text.ToStringOrNull();
            //ViewModel.IM = TxtInscricaoMunicipal.Text.ToStringOrNull();
            //ViewModel.Sexo = (Sexo?)DplSexo.SelectedValue.ToIntOrNull();
            //ViewModel.Abertura = DtpAbertura.Value;

            ViewModel.EmpresaEndereco = ViewEmpresaEndereco.ListView.Items.ToList();
            ViewModel.EmpresaCertificado = ViewEmpresaCertificado.ListView.Items.ToList();


            foreach (var item in ViewModel.EmpresaEndereco)
            {

            }

            foreach (var item in ViewModel.EmpresaCertificado)
            {

            }

            var HelpUpdate = new HelpUpdate();

            HelpUpdate.Add(ViewModel);

            var Changes = await HelpUpdate.SaveChanges();

            ViewModel = HelpUpdate.Bind<Empresa>(Changes[0]);


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

            var Query = new HelpQuery<Empresa>();

            Query.AddWhere("EmpresaID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            foreach (var item in ViewModel)
            {
                item.Ativo = false;
            }

            //await Query.Update(ViewModel, false);

        }
    }
}