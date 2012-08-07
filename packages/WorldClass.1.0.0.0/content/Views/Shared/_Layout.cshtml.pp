<!DOCTYPE html>
<html>
<head>
    <title>@ViewBag.Title</title>
    <link href="@Url.Content("~/Content/bootstrap.css")" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            padding-top: 60px;
            padding-bottom: 40px;
        }
    </style>
    <link href="@Url.Content("~/Content/bootstrap-responsive.css")" rel="stylesheet" type="text/css" />
    @RenderSection("Stylesheets", false)
</head>
<body>
    <div class="navbar navbar-fixed-top">
        <div class="navbar-inner">
            <div class="container">
                <a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse"><span
                    class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span>
                </a><a class="brand" href="@Url.Content("/")">Project name</a>
                
                
                @Html.Partial("_LogOnPartial")

                
                <div class="nav-collapse">
                    <ul class="nav">
                        <li class="active"><a href="@Url.Content("/")">Home</a></li>
                        <li><a href="#about">About</a></li>
                        <li><a href="#contact">Contact</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        @RenderSection("container", false)
        @if (!IsSectionDefined("container"))
        {                                            
            <!-- Main hero unit for a primary marketing message or call to action -->
                               
            if (!User.Identity.IsAuthenticated)
            {
            <div class="hero-unit">
                @RenderSection("container", false)
                @if (!IsSectionDefined("container"))
                {
                    <h1>
                        Hello, world!</h1>
                    <p>
                        This is a template for a simple marketing or informational website. It includes
                        a large callout called the hero unit and three supporting pieces of content. Use
                        it as a starting point to create something more unique.</p>
                    <p>
                        <a class="btn btn-primary btn-large">Learn more &raquo;</a></p>
                }
            </div>
            <!-- Example row of columns -->
            <div class="row">
                <div class="span4">
                    <h2>
                        Heading</h2>
                    <p>
                        Donec id elit non mi porta gravida at eget metus. Fusce dapibus, tellus ac cursus
                        commodo, tortor mauris condimentum nibh, ut fermentum massa justo sit amet risus.
                        Etiam porta sem malesuada magna mollis euismod. Donec sed odio dui.
                    </p>
                    <p>
                        <a class="btn" href="#">View details &raquo;</a></p>
                </div>
                <div class="span4">
                    <h2>
                        Heading</h2>
                    <p>
                        Donec id elit non mi porta gravida at eget metus. Fusce dapibus, tellus ac cursus
                        commodo, tortor mauris condimentum nibh, ut fermentum massa justo sit amet risus.
                        Etiam porta sem malesuada magna mollis euismod. Donec sed odio dui.
                    </p>
                    <p>
                        <a class="btn" href="#">View details &raquo;</a></p>
                </div>
                <div class="span4">
                    <h2>
                        Heading</h2>
                    <p>
                        Donec sed odio dui. Cras justo odio, dapibus ac facilisis in, egestas eget quam.
                        Vestibulum id ligula porta felis euismod semper. Fusce dapibus, tellus ac cursus
                        commodo, tortor mauris condimentum nibh, ut fermentum massa justo sit amet risus.</p>
                    <p>
                        <a class="btn" href="#">View details &raquo;</a></p>
                </div>
            </div>
            
            @RenderBody()
            
            <hr />
                
            }
            else
            {
                @RenderBody()
            }
                 
            
            <footer>
                <p>&copy; Company 2012</p>
              </footer>                                        
        
        }
    </div>
    <!-- /container -->
</body>
<script src="@Url.Content("~/Scripts/jquery-1.7.2.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery-ui-1.8.20.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.unobtrusive-ajax.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/bootstrap.js")" type="text/javascript"></script>
@RenderSection("Scripts", false)
</html>
