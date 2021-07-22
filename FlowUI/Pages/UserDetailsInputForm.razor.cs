﻿using AutoMapper;
using Flow.Core.DomainModels;
using Flow.Core.Mediate.UpsertUser;
using FlowUI.Utilities.LoggedInUserRequest;
using FlowUI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;


namespace FlowUI.Pages
{
    public partial class UserDetailsInputForm
    {
        [Inject]
        public IMediator _mediator { get; set; }
        [Inject]
        public IMapper _mapper { get; set; }

        [Inject]
        public NavigationManager _navigationManager { get; set; }

        private UserViewModel LoggedInUser { get; set; } = new UserViewModel();

        public UserDetailsInputForm() { }
        
        protected override async Task OnInitializedAsync()
        {
            LoggedInUser = await _mediator.Send(new GetLoggedInUserRequest());
        }

        protected async Task HandleValidSubmit()
        {
            await _mediator.Send(new UpsertUserRequest { User = _mapper.Map<User>(LoggedInUser) });
            _navigationManager.NavigateTo("/");
        }

        protected async Task HandleInvalidSubmit()
        {

        }

        // TODO: Find a nicer way to handle profile pictures.
        // TODO: Add validation of user input images.
        private async Task ProfilePictureSelected(InputFileChangeEventArgs evt)
        {
            var i = await evt.File.RequestImageFileAsync("jpg", 250, 250);

            var x = i.OpenReadStream();
            byte[] img = new byte[x.Length];
            await x.ReadAsync(img, 0, img.Length);

            LoggedInUser.ProfilePicture = img;

            //Image = "data:image/png;base64," + Convert.ToBase64String(img, 0, img.Length);
        }
    }
}
