using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{
    public partial class ViewPesquisaControl<Tipo> : ComponentBase
    {

        protected ViewModal ViewModal { get; set; }
        protected ViewPesquisaModal<Tipo> ViewPesquisaModal { get; set; }

        public List<string> Includes { get; set; } = new List<string>();
        public List<Where> Where { get; set; } = new List<Where>();

        public string PrimaryKey { get; set; }

        [Parameter] public string _Label { get; set; }
        [Parameter] public string _Title { get; set; }
        [Parameter] public EventCallback OnChange { get; set; }
        [Parameter] public EventCallback OnBeforePesquisar { get; set; }

        public string Label
        {
            get => _Label;
            set
            {
                _Label = value;
                StateHasChanged();
            }
        }

        public string Title
        {
            get => _Title;
            set {
                _Title = value;
                StateHasChanged();
            }
        }

        public ElementReference Element => TxtCodigo.Element;

        public string Value 
        {
            get => TxtCodigo.Text.ToStringOrNull();
            set => TxtCodigo.Text = value.ToStringOrNull();
        }
        public string Text
        {
            get => TxtDescricao.Text.ToStringOrNull();
            set => TxtDescricao.Text = value.ToStringOrNull();
        }

        public TextBox TxtCodigo { get; set; }
        public TextBox TxtDescricao { get; set; }

        public void Clear()
        {
            Value = null;
            Text = null;
        }

        protected void Page_Load()
        {

            ViewPesquisaModal.Columns.Clear();

            switch (typeof(Tipo).Name)
            {
                case "Pessoa":
                    PrimaryKey = "PessoaID";
                    ViewPesquisaModal.Columns.Add(new Column("PessoaID", "Código", typeof(int)));
                    ViewPesquisaModal.Columns.Add(new Column("NomeFantasia", "Nome", typeof(string)));
                    ViewPesquisaModal.Columns.Add(new Column("CNPJ", "CPF/CNPJ", typeof(string), "CNPJ_Formatado"));
                    ViewPesquisaModal.Refresh();
                    ViewPesquisaModal.DplCampo.SelectedValue = "NomeFantasia";
                    break;

                case "Produto":
                    PrimaryKey = "ProdutoID";
                    ViewPesquisaModal.Columns.Add(new Column("ProdutoID", "Código", typeof(int)));
                    ViewPesquisaModal.Columns.Add(new Column("Descricao", "Descrição", typeof(string)));
                    ViewPesquisaModal.Refresh();
                    ViewPesquisaModal.DplCampo.SelectedValue = "Descricao";
                    break;

                case "ContaBancaria":
                    PrimaryKey = "ContaBancariaID";
                    ViewPesquisaModal.Columns.Add(new Column("ContaBancariaID", "Código", typeof(int)));
                    ViewPesquisaModal.Columns.Add(new Column("Descricao", "Descrição", typeof(string)));
                    ViewPesquisaModal.Refresh();
                    ViewPesquisaModal.DplCampo.SelectedValue = "Descricao";
                    break;

                case "FormaPagamento":
                    PrimaryKey = "FormaPagamentoID";
                    ViewPesquisaModal.Columns.Add(new Column("FormaPagamentoID", "Código", typeof(int)));
                    ViewPesquisaModal.Columns.Add(new Column("Descricao", "Descrição", typeof(string)));
                    ViewPesquisaModal.Refresh();
                    ViewPesquisaModal.DplCampo.SelectedValue = "Descricao";
                    break;

                case "PlanoConta":
                    PrimaryKey = "PlanoContaID";
                    ViewPesquisaModal.Columns.Add(new Column("PlanoContaID", "Código", typeof(int)));
                    ViewPesquisaModal.Columns.Add(new Column("Descricao", "Descrição", typeof(string)));
                    ViewPesquisaModal.Refresh();
                    ViewPesquisaModal.DplCampo.SelectedValue = "Descricao";
                    break;

                case "CentroCusto":
                    PrimaryKey = "CentroCustoID";
                    ViewPesquisaModal.Columns.Add(new Column("CentroCustoID", "Código", typeof(int)));
                    ViewPesquisaModal.Columns.Add(new Column("Descricao", "Descrição", typeof(string)));
                    ViewPesquisaModal.Refresh();
                    ViewPesquisaModal.DplCampo.SelectedValue = "Descricao";
                    break;

                case "Municipio":
                    PrimaryKey = "MunicipioID";
                    ViewPesquisaModal.Columns.Add(new Column("MunicipioID", "Código", typeof(int)));
                    ViewPesquisaModal.Columns.Add(new Column("Nome", "Nome", typeof(string)));
                    ViewPesquisaModal.Refresh();
                    ViewPesquisaModal.DplCampo.SelectedValue = "Nome";
                    break;
            }

        }

        protected async Task TxtCodigo_Leave()
        {
            try
            {

                if (string.IsNullOrEmpty(TxtCodigo.Text))
                {
                    Clear();
                    await OnChange.InvokeAsync(null);
                    return;
                }

                await OnBeforePesquisar.InvokeAsync(null);

                var Query = new HelpQuery<Tipo>();

                foreach (var include in Includes)
                {
                    Query.AddInclude(include);
                }

                foreach (var where in Where)
                {
                    Query.AddWhere(where.Predicate, where.Args);
                }

                switch (typeof(Tipo).Name)
                {
                    case "Pessoa":

                        Query.AddWhere(PrimaryKey + " == @0", TxtCodigo.Text.ToIntOrNull());

                        var Pessoa = await Query.FirstOrDefault();

                        if (Pessoa == null)
                        {
                            await App.JSRuntime.InvokeVoidAsync("alert", "Não encontrado!");
                            Clear();
                            return;
                        }

                        TxtCodigo.Text = (Pessoa as Pessoa).PessoaID.ToStringOrNull();
                        TxtDescricao.Text = (Pessoa as Pessoa).NomeFantasia;

                        await OnChange.InvokeAsync(Pessoa);

                        break;

                    case "Produto":

                        Query.AddWhere(PrimaryKey + " == @0", TxtCodigo.Text.ToIntOrNull());

                        var Produto = await Query.FirstOrDefault();

                        if (Produto == null)
                        {
                            await App.JSRuntime.InvokeVoidAsync("alert", "Não encontrado!");
                            Clear();
                            return;
                        }

                        TxtCodigo.Text = (Produto as Produto).ProdutoID.ToStringOrNull();
                        TxtDescricao.Text = (Produto as Produto).Descricao;

                        await OnChange.InvokeAsync(Produto);

                        break;

                    case "ContaBancaria":

                        Query.AddWhere(PrimaryKey + " == @0", TxtCodigo.Text.ToIntOrNull());

                        var ContaBancaria = await Query.FirstOrDefault();

                        if (ContaBancaria == null)
                        {
                            await App.JSRuntime.InvokeVoidAsync("alert", "Não encontrado!");
                            Clear();
                            return;
                        }

                        TxtCodigo.Text = (ContaBancaria as ContaBancaria).ContaBancariaID.ToStringOrNull();
                        TxtDescricao.Text = (ContaBancaria as ContaBancaria).Descricao;

                        await OnChange.InvokeAsync(ContaBancaria);

                        break;

                    case "FormaPagamento":

                        Query.AddWhere(PrimaryKey + " == @0", TxtCodigo.Text.ToIntOrNull());

                        var FormaPagamento = await Query.FirstOrDefault();

                        if (FormaPagamento == null)
                        {
                            await App.JSRuntime.InvokeVoidAsync("alert", "Não encontrado!");
                            Clear();
                            return;
                        }

                        TxtCodigo.Text = (FormaPagamento as FormaPagamento).FormaPagamentoID.ToStringOrNull();
                        TxtDescricao.Text = (FormaPagamento as FormaPagamento).Descricao;

                        await OnChange.InvokeAsync(FormaPagamento);

                        break;

                    case "PlanoConta":

                        Query.AddWhere(PrimaryKey + " == @0", TxtCodigo.Text.ToIntOrNull());

                        var PlanoConta = await Query.FirstOrDefault();

                        if (PlanoConta == null)
                        {
                            await App.JSRuntime.InvokeVoidAsync("alert", "Não encontrado!");
                            Clear();
                            return;
                        }

                        TxtCodigo.Text = (PlanoConta as PlanoConta).PlanoContaID.ToStringOrNull();
                        TxtDescricao.Text = (PlanoConta as PlanoConta).Descricao;

                        await OnChange.InvokeAsync(PlanoConta);

                        break;

                    case "CentroCusto":

                        Query.AddWhere(PrimaryKey + " == @0", TxtCodigo.Text.ToIntOrNull());

                        var CentroCusto = await Query.FirstOrDefault();

                        if (CentroCusto == null)
                        {
                            await App.JSRuntime.InvokeVoidAsync("alert", "Não encontrado!");
                            Clear();
                            return;
                        }

                        TxtCodigo.Text = (CentroCusto as CentroCusto).CentroCustoID.ToStringOrNull();
                        TxtDescricao.Text = (CentroCusto as CentroCusto).Descricao;

                        await OnChange.InvokeAsync(CentroCusto);

                        break;

                    case "Municipio":

                        Query.AddWhere(PrimaryKey + " == @0", TxtCodigo.Text.ToIntOrNull());

                        var Municipio = await Query.FirstOrDefault();

                        if (Municipio == null)
                        {
                            await App.JSRuntime.InvokeVoidAsync("alert", "Não encontrado!");
                            Clear();
                            return;
                        }

                        TxtCodigo.Text = (Municipio as Municipio).MunicipioID.ToStringOrNull();
                        TxtDescricao.Text = (Municipio as Municipio).Nome;

                        await OnChange.InvokeAsync(Municipio);

                        break;
                }

            }
            catch (Exception ex)
            {
                await App.JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
        }

        protected async Task TxtCodigo_KeyUp(object args)
        {
            if (((KeyboardEventArgs)args).Key == "F2")
            {
                await BtnPesquisar_Click();
            }
        }

        public void AddInclude(string Include)
        {
            Includes.Add(Include);
        }

        public void AddWhere(string Predicate, params object[] Args)
        {

            var Where = new Where();

            Where.Predicate = Predicate;
            Where.Args = Args;

            this.Where.Add(Where);

        }

        protected async Task BtnPesquisar_Click()
        {
            try
            {

                await OnBeforePesquisar.InvokeAsync(null);

                await ViewModal.Show();
                ViewPesquisaModal.Where.Clear();
                ViewPesquisaModal.Where.AddRange(Where);
                await ViewPesquisaModal.TxtPesquisa_Input();
                ViewPesquisaModal.TxtPesquisa.Focus();
            }
            catch (Exception ex)
            {
                await App.JSRuntime.InvokeVoidAsync("alert", ex.Message);
            }
        }

        protected async Task ViewPesquisaModal_Click(object args)
        {
            await ViewModal.Hide();
            TxtCodigo.Text = args.GetType().GetProperty(PrimaryKey).GetValue(args).ToString();
            await TxtCodigo_Leave();
        }

        public void Focus()
        {
            Element.Focus();
        }

        protected void ViewModal_Hide()
        {
            TxtCodigo.Focus();
        }

    }
}