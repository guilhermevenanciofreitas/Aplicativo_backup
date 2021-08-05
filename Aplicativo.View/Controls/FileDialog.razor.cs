using Aplicativo.Utils.Models;
using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tewr.Blazor.FileReader;

namespace Aplicativo.View.Controls
{

    public class FileDialogControl : ComponentBase
    {

        [Parameter] public bool Multiple { get; set; }

        [Parameter] public string[] Accept { get; set; }

        protected string FilesAccepted => Accept == null ? "" : string.Join(",", Accept);

        [Parameter] public EventCallback OnChange { get; set; }

        [Inject] IFileReaderService fileReaderService { get; set; }

        protected ElementReference ElementReference;

        private List<Arquivo> Arquivos { get; set; } = new List<Arquivo>();

        public async Task<List<Arquivo>> OpenFileDialog(bool Multiple = false, string[] Accept = null)
        {
            try
            {

                this.Multiple = Multiple;
                this.Accept = Accept;

                StateHasChanged();


                await HelpLoading.Show();

                await App.JSRuntime.InvokeVoidAsync("ElementReference.Click", ElementReference);

                using (FinishConfirm = new CancellationTokenSource())
                {
                    await Task.Delay(-1, FinishConfirm.Token);
                }

                return Arquivos;

            }
            catch (Exception)
            {
                return Arquivos;
            }
            finally
            {
                await HelpLoading.Hide();
            }
        }

        protected async Task FileChange()
        {

            try
            {

                Arquivos = new List<Arquivo>();

                foreach (var file in await fileReaderService.CreateReference(ElementReference).EnumerateFilesAsync())
                {

                    var FileInfo = await file.ReadFileInfoAsync();

                    using (MemoryStream memoryStream = await file.CreateMemoryStreamAsync((int)FileInfo.Size))
                    {

                        Arquivos.Add(new Arquivo()
                        {
                            Anexo = memoryStream.ToArray(),
                            Nome = FileInfo.Name,
                            Tamanho = FileInfo.Size,
                        });

                    }
                }

                await fileReaderService.CreateReference(ElementReference).ClearValue();

                await OnChange.InvokeAsync(Arquivos);

                FinishConfirmToken();


            }
            catch (Exception ex)
            {

            }
        }


        private void FinishConfirmToken()
        {
            if (FinishConfirm?.Token.CanBeCanceled ?? false)
            {
                FinishConfirm.Cancel();
            }
        }

        private CancellationTokenSource FinishConfirm;

    }
}
