@if (Request.IsAuthenticated)
{
    <div class="btn-group pull-right">
        <a class="btn dropdown-toggle" data-toggle="dropdown" href="#"><i class="icon-user">
        </i>@User.Identity.Name <span class="caret"></span></a>
        <ul class="dropdown-menu">
            <li><a href="#">Profile</a></li>
            <li>
                @Html.ActionLink("Change Password", "ChangePassword", "Account")
            </li>
            <li class="divider"></li>
            <li><a href="@Url.Action("LogOff", "Account")">Sign Out</a></li>
        </ul>
    </div>
}
else
{
    <div class="pull-right">
        <ul class="nav">
            <li><a href="@Url.Action("Login", "Account")">Login</a></li>
            <li>@Html.ActionLink("Register", "Register", "Account")
            </li>
        </ul>
    </div>
}