using Aplicativo.Utils;
using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Controls;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Aplicativo.View.Layout.Component.ListView;
using Aplicativo.View.Layout.Component.ViewPage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Aplicativo.View.Pages.Configuracao.Empresas
{
    public class ViewEmpresaCertificadoPage : ComponentBase
    {

        public EmpresaCertificado ViewModel { get; set; } = new EmpresaCertificado();

        public ListItemViewLayout<EmpresaCertificado> ListView { get; set; }
        public EditItemViewLayout EditItemViewLayout { get; set; }

        #region Elements

        public Options OptTipo { get; set; }
        public FileDialog FileDialog { get; set; }
        public ViewModal ViewModalCertificado { get; set; }
        public TextBox TxtPassword { get; set; }
        public bool Confirmed { get; set; } = false;

        public TextBox TxtCertificado { get; set; }
        public TextBox TxtNome { get; set; }
        public TextBox TxtSerial { get; set; }
        public TextBox TxtExpira { get; set; }
        

        #endregion

        #region ListView
        protected async Task ViewLayout_ItemView(object args)
        {
            await ViewLayout_Carregar(args);
        }
        #endregion

        #region ViewPage
        protected void BtnLimpar_Click()
        {

            ViewModel = new EmpresaCertificado();

            ViewModel.Certificado = new Certificado();
            ViewModel.Certificado.Arquivo = new Arquivo();


            EditItemViewLayout.LimparCampos(this);

            OptTipo.Value = "1";

            //TxtSmtp.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            BtnLimpar_Click();

            if (args == null) return;

            ViewModel = (EmpresaCertificado)args;

            TxtCertificado.Text = ViewModel.Certificado.Arquivo.Nome;
            TxtNome.Text = ViewModel.Certificado.Nome.ToStringOrNull();
            TxtSerial.Text = ViewModel.Certificado.Serial.ToStringOrNull();
            TxtExpira.Text = ViewModel.Certificado.Expira?.ToString("dd/MM/yyyy");

        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            if (ViewModel.Certificado.Arquivo.Anexo == null)
                throw new EmptyException("Informe o certificado!");

            if (Convert.ToDateTime(TxtExpira.Text.ToStringOrNull()) < DateTime.Today)
                throw new EmptyException("O certificado está expirado!");


            ViewModel.Certificado.Nome = TxtNome.Text.ToStringOrNull();
            ViewModel.Certificado.Serial = TxtSerial.Text.ToStringOrNull();
            ViewModel.Certificado.Expira = Convert.ToDateTime(TxtExpira.Text.ToStringOrNull());


            if (EditItemViewLayout.ItemViewMode == ItemViewMode.New)
            {
                ListView.Items.Add(ViewModel);
            }

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected async Task ViewPageBtnExcluir_Click()
        {

            Excluir(new List<EmpresaCertificado>() { ViewModel });

            await EditItemViewLayout.ViewModal.Hide();

        }

        protected void ListViewBtnExcluir_Click(object args)
        {

            Excluir(((IEnumerable)args).Cast<EmpresaCertificado>().ToList());

        }

        public void Excluir(List<EmpresaCertificado> args)
        {
            foreach (var item in args)
            {
                ListView.Items.Remove(item);
            }
        }

        protected async Task OpenFileDialog()
        {
            try
            {

                Confirmed = false;

                var Arquivo = (await FileDialog.OpenFileDialog(Multiple: false, Accept: new string[] { ".pfx" })).FirstOrDefault();

                if (Arquivo != null)
                {

                    TxtPassword.Text = null;

                    await ViewModalCertificado.ShowAsync();

                    if (!Confirmed)
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(TxtPassword.Text))
                    {
                        throw new EmptyException("Informe a senha!", TxtPassword.Element);
                    }

                    var Request = new Request();

                    Request.Parameters.Add(new Parameters("Certificate", Arquivo.Anexo));
                    Request.Parameters.Add(new Parameters("Password", TxtPassword.Text));

                    var Certificado = await HelpHttp.Send<Certificado>("api/Certificate/InfoCertificate", Request);


                    TxtCertificado.Text = Arquivo.Nome;

                    TxtNome.Text = Certificado.Nome.ToStringOrNull();
                    TxtSerial.Text = Certificado.Serial.ToStringOrNull();
                    TxtExpira.Text = Certificado.Expira?.ToString("dd/MM/yyyy");

                    ViewModel.Certificado.Senha = TxtPassword.Text;

                    ViewModel.Certificado.Arquivo.Nome = Arquivo.Nome;
                    ViewModel.Certificado.Arquivo.Tamanho = Arquivo.Tamanho;

                    ViewModel.Certificado.Arquivo.Anexo = Arquivo.Anexo;


                }

            }
            catch (EmptyException)
            {

            }
            catch (ErrorException ex)
            {
                await App.JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        protected async Task BtnConfirmar_Click()
        {
            Confirmed = true;
            await ViewModalCertificado.Hide();
        }

        #endregion

    }
}