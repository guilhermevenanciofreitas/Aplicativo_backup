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

        public TextBox TxtChaveAcesso { get; set; }
        public DropDownList DplModelo { get; set; }
        public TextBox TxtNaturezaOperacao { get; set; }

        public TextBox TxtNumero { get; set; }
        public TextBox TxtSerie { get; set; }
        public DropDownList DplTipo { get; set; }
        public DropDownList DplFinalidade { get; set; }
        public DateTimePicker DtpEmissao { get; set; }
        public DateTimePicker DtpEntradaSaida { get; set; }

        public NumericBox TxtBaseCalculoICMS { get; set; }
        public NumericBox TxtValorICMS { get; set; }
        public NumericBox TxtBaseCalculoICMSST { get; set; }
        public NumericBox TxtValorICMSST { get; set; }
        public NumericBox TxtAproxTributos { get; set; }
        public NumericBox TxtTotalProdutos { get; set; }

        public NumericBox TxtValorFrete { get; set; }
        public NumericBox TxtValorSeguro { get; set; }
        public NumericBox TxtDesconto { get; set; }
        public NumericBox TxtOutrasDespAcess { get; set; }
        public NumericBox TxtValorIPI { get; set; }
        public NumericBox TxtTotalNota { get; set; }

        public TextArea TxtInformacoesComplementares { get; set; }


        public TextBox Emit_TxtCNPJ { get; set; }
        public TextBox Emit_TxtRazaoSocial { get; set; }
        public DropDownList Emit_TxtUF { get; set; }
        public ViewPesquisa<Municipio> Emit_ViewPesquisaMunicipio { get; set; }



        public TextBox Dest_TxtCNPJ { get; set; }
        public TextBox Dest_TxtRazaoSocial { get; set; }



        public ViewNotaFiscalItem ViewNotaFiscalItem { get; set; }

        #endregion

        protected void InitializeComponents()
        {

            DplModelo.LoadDropDownList("Codigo", "Descricao", new DropDownListItem(null, "[Selecione]"), HelpParametros.Parametros.NotaFiscalModelo.OrderBy(c => c.Codigo).ToList());

            DplTipo.Items.Clear();
            DplTipo.Add(null, "[Selecione]");
            DplTipo.Add("0", "0 - Entrada");
            DplTipo.Add("1", "1 - Saída");

            DplFinalidade.Items.Clear();
            DplFinalidade.Add(null, "[Selecione]");
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

            Query.AddInclude("NotaFiscalItem.CFOP");
            Query.AddInclude("NotaFiscalItem.NCM");
            Query.AddInclude("NotaFiscalItem.CEST");

            Query.AddInclude("NotaFiscalItem.CST_ICMS");
            Query.AddInclude("NotaFiscalItem.CSOSN_ICMS");
            Query.AddInclude("NotaFiscalItem.CST_IPI");
            Query.AddInclude("NotaFiscalItem.CST_PIS");
            Query.AddInclude("NotaFiscalItem.CST_COFINS");

            Query.AddWhere("NotaFiscalID == @0", ((NotaFiscal)args).NotaFiscalID);
            
            ViewModel = await Query.FirstOrDefault();


            #region Principal
            TxtChaveAcesso.Text = ViewModel.chNFe.StringFormat("#### #### #### #### #### #### #### #### #### #### ####");
            DplModelo.SelectedValue = ViewModel.mod.ToStringOrNull();
            TxtNaturezaOperacao.Text = ViewModel.natOp.ToStringOrNull();

            TxtNumero.Text = ViewModel.nNF.ToStringOrNull();
            TxtSerie.Text = ViewModel.serie.ToStringOrNull();
            DplTipo.SelectedValue = ViewModel.tpNF.ToStringOrNull();
            DplFinalidade.SelectedValue = ViewModel.finNFe.ToStringOrNull();
            DtpEmissao.Value = ViewModel.dhEmi;
            DtpEntradaSaida.Value = ViewModel.dhSaiEnt;

            TxtBaseCalculoICMS.Value = ViewModel.vBC ?? 0;
            TxtValorICMS.Value = ViewModel.vICMS ?? 0;
            TxtBaseCalculoICMSST.Value = ViewModel.vBCST ?? 0;
            TxtValorICMSST.Value = ViewModel.vST ?? 0;
            TxtAproxTributos.Value = ViewModel.vTotTrib ?? 0;
            TxtTotalProdutos.Value = ViewModel.vProd ?? 0;

            TxtValorFrete.Value = ViewModel.vFrete ?? 0;
            TxtValorSeguro.Value = ViewModel.vSeg ?? 0;
            TxtDesconto.Value = ViewModel.vDesc ?? 0;
            TxtOutrasDespAcess.Value = ViewModel.vOutro ?? 0;
            TxtValorIPI.Value = ViewModel.vIPI ?? 0;
            TxtTotalNota.Value = ViewModel.vNF ?? 0;



            TxtInformacoesComplementares.Text = ViewModel.infCpl.ToStringOrNull();
            #endregion


            Emit_TxtCNPJ.Text = ViewModel.CNPJCPF.ToStringOrNull();
            Emit_TxtRazaoSocial.Text = ViewModel.xNome.ToStringOrNull();


            Dest_TxtCNPJ.Text = ViewModel.dest_CNPJCPF.ToStringOrNull();
            Dest_TxtRazaoSocial.Text = ViewModel.dest_xNome.ToStringOrNull();



            ViewNotaFiscalItem.ListView.Items = ViewModel.NotaFiscalItem.ToList();
            
        }

        protected async Task BtnLimpar_Click()
        {

            EditItemViewLayout.LimparCampos(this);

            DplTipo.SelectedValue = "0";
            DplFinalidade.SelectedValue = "1";

            ViewNotaFiscalItem.ListView.Items = new List<NotaFiscalItem>();

            await TabSet.Active("Principal");

            TxtChaveAcesso.Focus();

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


            #region Principal
            ViewModel.chNFe = TxtChaveAcesso.Text?.Replace(" ", "");
            ViewModel.mod = DplModelo.SelectedValue.ToStringOrNull();
            ViewModel.natOp = TxtNaturezaOperacao.Text.ToStringOrNull();

            ViewModel.nNF = TxtNumero.Text.ToIntOrNull();
            ViewModel.serie = TxtSerie.Text.ToIntOrNull();

            ViewModel.tpNF = DplTipo.SelectedValue.ToByteOrNull();
            ViewModel.finNFe = DplFinalidade.SelectedValue.ToByteOrNull();
            ViewModel.dhEmi = DtpEmissao.Value;
            ViewModel.dhSaiEnt = DtpEntradaSaida.Value;

            ViewModel.infCpl = TxtInformacoesComplementares.Text.ToStringOrNull();


            ViewModel.vBC = TxtBaseCalculoICMS.Value;
            ViewModel.vICMS = TxtValorICMS.Value;
            ViewModel.vBCST = TxtBaseCalculoICMSST.Value;
            ViewModel.vST = TxtValorICMSST.Value;
            ViewModel.vTotTrib = TxtAproxTributos.Value;
            ViewModel.vProd = TxtTotalProdutos.Value;

            ViewModel.vFrete = TxtValorFrete.Value;
            ViewModel.vSeg = TxtValorSeguro.Value;
            ViewModel.vDesc = TxtDesconto.Value;
            ViewModel.vOutro = TxtOutrasDespAcess.Value;
            ViewModel.vIPI = TxtValorIPI.Value;
            ViewModel.vNF = TxtTotalNota.Value;

            #endregion

            ViewModel.CNPJCPF = Emit_TxtCNPJ.Text.ToStringOrNull();
            ViewModel.xNome = Emit_TxtRazaoSocial.Text.ToStringOrNull();


            ViewModel.dest_CNPJCPF = Dest_TxtCNPJ.Text.ToStringOrNull();
            ViewModel.dest_xNome = Dest_TxtRazaoSocial.Text.ToStringOrNull();


            ViewModel.NotaFiscalItem = ViewNotaFiscalItem.ListView.Items.ToList();

            foreach(var item in ViewModel.NotaFiscalItem)
            {
                item.CFOP = null;
                item.NCM = null;
                item.CEST = null;
                item.CST_ICMS = null;
                item.CSOSN_ICMS = null;
                item.CST_IPI = null;
                item.CST_PIS = null;
                item.CST_COFINS = null;
            }

            var HelpUpdate = new HelpUpdate();

            HelpUpdate.Add(ViewModel);

            var Changes = await HelpUpdate.SaveChanges();

            ViewModel = HelpUpdate.Bind<NotaFiscal>(Changes[0]);

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