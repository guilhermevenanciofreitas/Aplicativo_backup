using Aplicativo.Utils.Helpers;
using Aplicativo.Utils.Models;
using Aplicativo.Utils.WebServices;
using Aplicativo.View.Helpers;
using Aplicativo.View.Helpers.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Skclusive.Material.Text;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{

    public class TextBoxCEPComponent : ComponentBase
    {

        protected TextField TextField;

        public ElementReference Element;

        [Parameter] public TextBoxType _Type { get; set; } = TextBoxType.String;
        [Parameter] public string _Label { get; set; }
        [Parameter] public string _Text { get; set; }
        [Parameter] public string _PlaceHolder { get; set; }
        [Parameter] public bool _ReadOnly { get; set; }
        [Parameter] public CharacterCasing _CharacterCasing { get; set; } = CharacterCasing.UpperCase;

        [Parameter] public EventCallback OnInput { get; set; }
        [Parameter] public EventCallback OnKeyUp { get; set; }

        [Parameter] public EventCallback OnSuccess { get; set; }
        [Parameter] public EventCallback OnError { get; set; }

        public string Label
        {
            get
            {
                return _Label;
            }
            set
            {
                _Label = value;
                StateHasChanged();
            }
        }

        public string Text
        {
            get
            {
                switch (_CharacterCasing)
                {
                    case CharacterCasing.LowerCase:
                        return _Text?.Replace(".", "")?.Replace("-", "")?.ToLower();
                    case CharacterCasing.UpperCase:
                        return _Text?.Replace(".", "")?.Replace("-", "")?.ToUpper();
                    default:
                        return _Text?.Replace(".", "")?.Replace("-", "");
                }
            }
            set
            {
                switch (_CharacterCasing)
                {
                    case CharacterCasing.LowerCase:
                        _Text = value?.ToLower();
                        break;
                    case CharacterCasing.UpperCase:
                        _Text = value?.ToUpper();
                        break;
                    default:
                        _Text = value;
                        break;
                }
                StateHasChanged();
            }
        }

        public string PlaceHolder
        {
            get
            {
                return _PlaceHolder;
            }
            set
            {
                _PlaceHolder = value;
                StateHasChanged();
            }
        }

        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
                StateHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {

                    Text = _Text;
                    PlaceHolder = _PlaceHolder;

                    await App.JSRuntime.InvokeVoidAsync("ElementReference.Mask", Element, "99.999-999");

                }
                catch (Exception ex)
                {
                    await App.JSRuntime.InvokeVoidAsync("alert", ex.Message);
                }
            }
        }

        public void Focus()
        {
            Element.Focus();
        }

        protected async Task TextBox_Input(ChangeEventArgs args)
        {
            try
            {
                Text = args.Value.ToString();
                await OnInput.InvokeAsync(args);
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        protected async Task TextBox_KeyUp(KeyboardEventArgs args)
        {
            try
            {
                await OnKeyUp.InvokeAsync(args);
            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
        }

        protected async Task BtnPesquisar_Click()
        {
            try
            {

                if (string.IsNullOrEmpty(Text))
                {
                    throw new EmptyException("Informe o CEP!", Element);
                }

                var CEP = Text.Replace(".", "").Replace("-", "");

                if (CEP.Length != 8)
                {
                    await OnError.InvokeAsync("CEP deve possuir 8 caracteres!");
                    return;
                }

                await HelpLoading.Show("Consultando CEP...");

                //WebResponse oWebResponse = WebRequest.Create("https://viacep.com.br/ws/" + CEP + "/json/").GetResponse();

                //using StreamReader oStreamReader = new StreamReader(oWebResponse.GetResponseStream());

                var Result = await App.Http.GetAsync("https://viacep.com.br/ws/" + CEP + "/json/");

                ViaCEP result = JsonConvert.DeserializeObject<ViaCEP>(await Result.Content.ReadAsStringAsync());

                if (result == null || result.erro)
                {
                    await OnError.InvokeAsync("CEP não encontrado!");
                    return;
                }

                var Query = new HelpQuery<Municipio>();

                Query.AddWhere("IBGE == @0", Convert.ToInt32(result.ibge));

                var Municipio = await Query.FirstOrDefault();

                result.logradouro = result.logradouro?.ToUpper();
                result.complemento = result.complemento?.ToUpper();
                result.bairro = result.bairro?.ToUpper();
                result.UF = result.UF?.ToUpper();

                result.municipio = Municipio?.Nome;
                result.MunicipioID = Municipio?.MunicipioID;
                result.EstadoID = Municipio?.EstadoID;

                await OnSuccess.InvokeAsync(result);

            }
            catch (Exception ex)
            {
                await HelpErro.Show(new Error(ex));
            }
            finally
            {
                await HelpLoading.Hide();
            }
        }



    }
}