using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Cadastros.Pessoas
{
    public partial class ViewPessoaPage<TValue> : HelpComponent
    {

        [Parameter] public string Title { get; set; }
        [Parameter] public Tipo Tipo { get; set; }

        [Parameter]
        public ListItemViewLayout<TValue> ListItemViewLayout { get; set; }
        public EditItemViewLayout<Pessoa> EditItemViewLayout { get; set; }

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

        protected void ViewLayout_PageLoad()
        {

            DplTipo.Items.Clear();
            DplTipo.Add(((int)TipoPessoa.Fisica).ToString(), "Fisíca");
            DplTipo.Add(((int)TipoPessoa.Juridica).ToString(), "Jurídica");

            DplTipo.SelectedValue = ((int)TipoPessoa.Juridica).ToString();

            DplSexo.Items.Clear();
            DplSexo.Add(null, "[Selecione]");
            DplSexo.Add(((int)Sexo.Masculino).ToString(), "Masculino");
            DplSexo.Add(((int)Sexo.Feminino).ToString(), "Feminino");

        }
       

        protected async Task ViewLayout_Limpar()
        {

            await EditItemViewLayout.LimparCampos(this);

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

            ViewPessoaVendedor.ListItemViewLayout.ListItemView = new List<PessoaVendedor>();
            ViewPessoaVendedor.ListItemViewLayout.ListItemView.Add(new PessoaVendedor() { });
            ViewPessoaVendedor.ListItemViewLayout.Refresh();

            ViewPessoaEndereco.ListItemViewLayout.ListItemView = new List<PessoaEndereco>();
            ViewPessoaEndereco.ListItemViewLayout.Refresh();

            ViewPessoaContato.ListItemViewLayout.ListItemView = new List<PessoaContato>();
            ViewPessoaContato.ListItemViewLayout.Refresh();

            TxtCNPJ.Focus();

            await TabSet.Active("Principal");

        }

        protected void DplTipo_Change(ChangeEventArgs args)
        {

            if ((TipoPessoa)args?.Value.ToIntOrNull() == TipoPessoa.Fisica)
            {
                TxtCNPJ.SetMask("999.999.999-99");
                TxtRazaoSocial.Label = "Nome completo";
                TxtNomeFantasia.Label = "Apelido";
                TxtInscricaoEstadual.Label = "RG";
                DtpAbertura.Label = "Data nascimento";
            }
            else
            {
                TxtCNPJ.SetMask("99.999.999/9999");
                TxtRazaoSocial.Label = "Razão social";
                TxtNomeFantasia.Label = "Nome fantasia";
                TxtInscricaoEstadual.Label = "Insc. Estadual";
                DtpAbertura.Label = "Data abertura";
            }

            TxtCNPJ.Text = null;
            TxtCNPJ.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            var Query = new HelpQuery(typeof(TValue).Name);

            Query.AddInclude("PessoaEndereco");
            Query.AddInclude("PessoaEndereco.Endereco");

            Query.AddInclude("PessoaContato");
            Query.AddInclude("PessoaContato.Contato");

            Query.AddWhere("PessoaID == @0", ((Pessoa)args)?.PessoaID);
            Query.AddTake(1);

            EditItemViewLayout.ViewModel = await EditItemViewLayout.Carregar<Pessoa>(Query);


            TxtCodigo.Text = EditItemViewLayout.ViewModel.PessoaID.ToStringOrNull();
            DplTipo.SelectedValue = ((int?)EditItemViewLayout.ViewModel.TipoPessoaID).ToStringOrNull();
            TxtRazaoSocial.Text = EditItemViewLayout.ViewModel.RazaoSocial.ToStringOrNull();
            TxtNomeFantasia.Text = EditItemViewLayout.ViewModel.NomeFantasia.ToStringOrNull();
            ChkCliente.Checked = EditItemViewLayout.ViewModel.IsCliente.ToBoolean();
            ChkFornecedor.Checked = EditItemViewLayout.ViewModel.IsFornecedor.ToBoolean();
            ChkTransportadora.Checked = EditItemViewLayout.ViewModel.IsTransportadora.ToBoolean();
            ChkFuncionario.Checked = EditItemViewLayout.ViewModel.IsFuncionario.ToBoolean();


            TxtInscricaoEstadual.Text = EditItemViewLayout.ViewModel.IE.ToStringOrNull();
            TxtInscricaoMunicipal.Text = EditItemViewLayout.ViewModel.IM.ToStringOrNull();
            DplSexo.SelectedValue = ((int?)EditItemViewLayout.ViewModel.Sexo).ToStringOrNull();
            DtpAbertura.Value = EditItemViewLayout.ViewModel.Abertura;


            ViewPessoaEndereco.ListItemViewLayout.ListItemView = EditItemViewLayout.ViewModel.PessoaEndereco.ToList();
            ViewPessoaEndereco.ListItemViewLayout.Refresh();

            ViewPessoaContato.ListItemViewLayout.ListItemView = EditItemViewLayout.ViewModel.PessoaContato.ToList();
            ViewPessoaContato.ListItemViewLayout.Refresh();

            //await JSRuntime.InvokeVoidAsync("window.history.pushState", null, null, Path.Combine(NavigationManager.ToBaseRelativePath(NavigationManager.Uri), EditItemViewLayout.ViewModel.PessoaID.ToString()));

        }

        protected async Task ViewLayout_Salvar()
        {

            if (string.IsNullOrEmpty(TxtRazaoSocial.Text))
            {
                await TabSet.Active("Principal");
                throw new EmptyException("Informe a razão social!", TxtRazaoSocial.Element);
            }

            EditItemViewLayout.ViewModel.PessoaID = TxtCodigo.Text.ToIntOrNull();
            EditItemViewLayout.ViewModel.TipoPessoaID = (TipoPessoa?)DplTipo.SelectedValue.ToIntOrNull();
            EditItemViewLayout.ViewModel.RazaoSocial = TxtRazaoSocial.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.NomeFantasia = TxtNomeFantasia.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.IsCliente = ChkCliente.Checked;
            EditItemViewLayout.ViewModel.IsFornecedor = ChkFornecedor.Checked;
            EditItemViewLayout.ViewModel.IsTransportadora = ChkTransportadora.Checked;
            EditItemViewLayout.ViewModel.IsFuncionario = ChkFuncionario.Checked;

            EditItemViewLayout.ViewModel.IE = TxtInscricaoEstadual.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.IM = TxtInscricaoMunicipal.Text.ToStringOrNull();
            EditItemViewLayout.ViewModel.Sexo = (Sexo?)DplSexo.SelectedValue.ToIntOrNull();
            EditItemViewLayout.ViewModel.Abertura = DtpAbertura.Value;

            EditItemViewLayout.ViewModel.PessoaEndereco = ViewPessoaEndereco.ListItemViewLayout.ListItemView;

            EditItemViewLayout.ViewModel.PessoaContato = ViewPessoaContato.ListItemViewLayout.ListItemView;


            EditItemViewLayout.ViewModel = await EditItemViewLayout.Update(EditItemViewLayout.ViewModel);


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
                await EditItemViewLayout.Carregar(EditItemViewLayout.ViewModel);
            else
                EditItemViewLayout.ViewModal.Hide();
          
            
        }

        protected async Task ViewLayout_Excluir()
        {
            await EditItemViewLayout.Delete(EditItemViewLayout.ViewModel);
        }
    }
}