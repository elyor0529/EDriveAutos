<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="Edrive.Edrivie_Service_Ref" %>
<%@ Import Namespace="Edrive.Models" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

    
    <script src="Scripts/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.raty.min.js" type="text/javascript"></script>



<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<title>Edriveautos.com</title>
 

       
   
</head>
 <body>
   

<div id="star"></div> 
<script type="text/javascript">

    $('#star').raty({
        click: function (score, evt) {
            alert('ID: ' + $(this).attr('id') + '\nscore: ' + score + '\nevent: ' + evt);
        }
    });  </script>
    </body>
</html>
