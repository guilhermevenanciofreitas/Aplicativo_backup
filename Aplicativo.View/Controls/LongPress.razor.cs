﻿using Aplicativo.View.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace Aplicativo.View.Controls
{
    public class LongPressControl : HelpComponent
    {

        [Parameter] public int Time { get; set; } = 500;
        [Parameter] public EventCallback OnLongPress { get; set; }
        [Parameter] public EventCallback OnPress { get; set; }

        [Parameter] public RenderFragment Content { get; set; }

        protected bool IsPressedLong = false;

        protected async void MouseDown()
        {
            try
            {
                IsPressedLong = false;
                await JSRuntime.InvokeVoidAsync("LongPress.MouseDown", Time);
                await OnLongPress.InvokeAsync(null);
                IsPressedLong = true;
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
        }

        protected async void MouseUp()
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("LongPress.MouseUp");
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
        }

        protected async void _OnPress()
        {
            try
            {
                if (IsPressedLong == true) return;

                await OnPress.InvokeAsync(null);
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error: " + ex.Message);
            }
        }

    }
}