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

namespace Aplicativo.View.Pages.Fiscal.NotasFiscais
{
    public partial class ViewNotaFiscalPage : ComponentBase
    {

        public NotaFiscal ViewModel = new NotaFiscal();

        [Parameter] public ListItemViewLayout<NotaFiscal> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements
        public TabSet TabSet { get; set; }

        public TextBox TxtNumero { get; set; }
        public TextBox TxtSerie { get; set; }
        public DropDownList DplTipo { get; set; }
        public DropDownList DplFinalidade { get; set; }
        public DateTimePicker DtpEmissao { get; set; }
        public DateTimePicker DtpEntradaSaida { get; set; }



        public DropDownList Emit_TxtUF { get; set; }
        public ViewPesquisa<Municipio> Emit_ViewPesquisaMunicipio { get; set; }

        //public ViewUsuarioEmail ViewUsuarioEmail { get; set; }
        #endregion

        protected void InitializeComponents()
        {

            DplTipo.Items.Clear();
            DplTipo.Add("0", "0 - Entrada");
            DplTipo.Add("1", "1 - Saída");

            DplFinalidade.Items.Clear();
            DplFinalidade.Add("1", "1 - Normal");
            DplFinalidade.Add("2", "2 - Complementar");
            DplFinalidade.Add("3", "3 - Ajuste");
            DplFinalidade.Add("4", "4 - Devolução de merc.");


            Emit_TxtUF.LoadDropDownList("EstadoID", "UF", null, HelpParametros.Parametros.Estado.OrderBy(c => c.UF).ToList());

        }

        protected async Task Page_Load(object args)
        {

            InitializeComponents();

            await BtnLimpar_Click();

            if (args == null) return;

            var Query = new HelpQuery<NotaFiscal>();

            Query.AddInclude("NotaFiscalItem");
            Query.AddWhere("NotaFiscalID == @0", ((NotaFiscal)args).NotaFiscalID);
            
            ViewModel = await Query.FirstOrDefault();

            //TxtCodigo.Text = ViewModel.UsuarioID.ToStringOrNull();
            //TxtLogin.Text = ViewModel.Login.ToStringOrNull();
            //TxtSenha.Text = ViewModel.Senha.ToStringOrNull();
            //TxtConfirmarSenha.Text = ViewModel.Senha.ToStringOrNull();

            //ViewUsuarioEmail.ListView.Items = ViewModel.UsuarioEmail.ToList();
            
        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.LimparCampos(this);

            //ViewUsuarioEmail.ListView.Items = new List<UsuarioEmail>();

            await TabSet.Active("Principal");

            //TxtLogin.Focus();

        }

        protected async Task BtnSalvar_Click()
        {

            //if (string.IsNullOrEmpty(TxtLogin.Text))
            //{
            //    await TabSet.Active("Principal");
            //    throw new EmptyException("Informe o login!", TxtLogin.Element);
            //}

            //if (TxtSenha.Text != TxtConfirmarSenha.Text)
            //{
            //    await TabSet.Active("Principal");
            //    throw new EmptyException("A confirmação da senha está diferente da senha informada!", TxtConfirmarSenha.Element);
            //}

            //ViewModel.UsuarioID = TxtCodigo.Text.ToIntOrNull();
            //ViewModel.Login = TxtLogin.Text.ToStringOrNull();
            //ViewModel.Senha = TxtSenha.Text.ToStringOrNull();

            //ViewModel.UsuarioEmail = ViewUsuarioEmail.ListView.Items.ToList();


            //var Query = new HelpQuery<NotaFiscal>();

            //ViewModel = await Query.Update(ViewModel);

            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                EditItemViewLayout.ItemViewMode = ItemViewMode.Edit;
                //TxtCodigo.Text = ViewModel.NotaFiscalID.ToStringOrNull();
            }
            else
            {
                await EditItemViewLayout.ViewModal.Hide();
            }

        }

        protected async Task BtnExcluir_Click()
        {

            //await Excluir(new List<int> { TxtCodigo.Text.ToInt() });

            await EditItemViewLayout.ViewModal.Hide();

        }

        public async Task Excluir(List<int> args)
        {

            var Query = new HelpQuery<Usuario>();

            Query.AddWhere("UsuarioID IN (" + string.Join(",", args.ToArray()) + ")");

            var ViewModel = await Query.ToList();

            foreach (var item in ViewModel)
            {
                item.Ativo = false;
            }

            //await Query.Update(ViewModel, false);

        }




        #region Emitente
        protected void Emit_TxtUF_Change(object args)
        {

            var Predicate = "EstadoID == @0";

            Emit_ViewPesquisaMunicipio.Where.Remove(Emit_ViewPesquisaMunicipio.Where.FirstOrDefault(c => c.Predicate == Predicate));
            Emit_ViewPesquisaMunicipio.AddWhere(Predicate, ((ChangeEventArgs)args).Value.ToString());

            Emit_ViewPesquisaMunicipio.Clear();

        }
        #endregion


    }
}