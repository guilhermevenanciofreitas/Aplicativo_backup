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


        public TextBox TxtSmtp { get; set; }
        public TextBox TxtPorta { get; set; }
        public TextBox TxtEmail { get; set; }
        public TextBox TxtSenha { get; set; }
        public TextBox TxtConfirmarSenha { get; set; }

        public CheckBox ChkSSL { get; set; }
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

            EditItemViewLayout.LimparCampos(this);

            ChkSSL.Checked = true;

            TxtSmtp.Focus();

        }

        protected async Task ViewLayout_Carregar(object args)
        {

            await EditItemViewLayout.Show(args);

            BtnLimpar_Click();

            if (args == null) return;

            ViewModel = (EmpresaCertificado)args;

            //TxtSmtp.Text = ViewModel.Smtp.ToStringOrNull();
            //TxtPorta.Text = ViewModel.Porta.ToStringOrNull();
            //TxtEmail.Text = ViewModel.Email.ToStringOrNull();
            //TxtSenha.Text = ViewModel.Senha.ToStringOrNull();
            //TxtConfirmarSenha.Text = ViewModel.Senha.ToStringOrNull();
            //ChkSSL.Checked = ViewModel.Ssl.ToBoolean();

        }

        protected async Task ViewPageBtnSalvar_Click()
        {

            //if (string.IsNullOrEmpty(TxtSmtp.Text))
            //    throw new EmptyException("Informe o SMTP!", TxtSmtp.Element);

            //if (string.IsNullOrEmpty(TxtPorta.Text))
            //    throw new EmptyException("Informe a porta!", TxtPorta.Element);

            //if (string.IsNullOrEmpty(TxtEmail.Text))
            //    throw new EmptyException("Informe o email!", TxtEmail.Element);

            //if (string.IsNullOrEmpty(TxtSenha.Text))
            //    throw new EmptyException("Informe a senha!", TxtSenha.Element);

            //if (TxtSenha.Text != TxtConfirmarSenha.Text)
            //    throw new EmptyException("A confirmação da senha está diferente da senha informada!", TxtConfirmarSenha.Element);

            //ViewModel.Smtp = TxtSmtp.Text.ToStringOrNull();
            //ViewModel.Porta = TxtPorta.Text.ToIntOrNull();
            //ViewModel.Email = TxtEmail.Text.ToStringOrNull();
            //ViewModel.Senha = TxtSenha.Text.ToStringOrNull();
            //ViewModel.Ssl = ChkSSL.Checked;

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

        //protected void LoadFiles(InputFileChangeEventArgs e)
        //{
        //    isLoading = true;
        //    loadedFiles.Clear();

        //    foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
        //    {
        //        try
        //        {
        //            loadedFiles.Add(file);
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.LogError("File: {Filename} Error: {Error}",
        //                file.Name, ex.Message);
        //        }
        //    }

        //    isLoading = false;
        //}

        #endregion

    }
}