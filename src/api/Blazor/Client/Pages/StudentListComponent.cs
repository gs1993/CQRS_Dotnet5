using Blazor.Client.Utils;
using Dto.Student;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.Client.Pages
{
    public class StudentListComponent : BasePageComponent
    {
        public StudentListComponent(HttpClient httpClient) : base(httpClient)
        {
        }

        public IReadOnlyList<StudentDto> Students { get; private set; }

        protected override async Task OnInitializedAsync()
        {

        }
    }
}
