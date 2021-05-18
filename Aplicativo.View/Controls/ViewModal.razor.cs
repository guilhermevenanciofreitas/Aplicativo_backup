using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicativo.View.Controls
{
    public class ViewModalControl : ComponentBase
    {

        protected ElementReference DivModal { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public bool FullScreen { get; set; } = true;

        [Parameter] public int ZIndex { get; set; } = 8000;

        public bool Open { get; set; } = false;

        private bool Confirmed { get; set; }

        [Parameter] public bool Overlay { get; set; } = true;

        [Parameter] public EventCallback OnLoad { get; set; }

        [Parameter] public EventCallback OnHide { get; set; }

        [Parameter] public EventCallback OnConfirm { get; set; }

        public void Show()
        {

            ExecutingAsync = false;

            Confirmed = false;
            Open = true;
            StateHasChanged();

            OnLoad.InvokeAsync(null);

        }

        public void Hide()
        {
            Confirmed = false;
            Open = false;
            if (ExecutingAsync) FinishConfirmToken();

            OnHide.InvokeAsync(null);

        }

        public void Confirm()
        {
            Confirmed = true;
            Open = false;
            FinishConfirmToken();

            OnConfirm.InvokeAsync(null);

        }

        private void FinishConfirmToken()
        {

            if (FinishConfirm.Token.CanBeCanceled)
            {
                FinishConfirm.Cancel();
            }

            RaiseChange();

            StateHasChanged();

        }





        [Parameter]
        public EventCallback OnStateChanged { get; set; }

        internal async virtual void RaiseChange()
        {
            await OnStateChanged.InvokeAsync(null);
        }


        private CancellationTokenSource FinishConfirm;

        private string _stationName;

        public string StationName
        {
            get => _stationName;
            set
            {
                _stationName = value;
                RaiseChange();
            }
        }

        public Worker Working() => new Worker(this);



        private bool ExecutingAsync { get; set; } = false;

        public async Task<bool> ShowAsync()
        {
            try
            {

                ExecutingAsync = true;

                Confirmed = false;

                Open = true;

                using (FinishConfirm = new CancellationTokenSource())
                {
                    await Task.Delay(-1, FinishConfirm.Token);
                }

            }
            catch (TaskCanceledException) 
            {
                
            }

            ExecutingAsync = false;

            return Confirmed;

        }

    }


    public sealed class Worker : IDisposable
    {
        private ViewModalControl _parent;

        public Worker(ViewModalControl parent)
        {
            _parent = parent;
            _parent.RaiseChange();
        }

        public void Dispose()
        {
            _parent.RaiseChange();
        }
    }

}