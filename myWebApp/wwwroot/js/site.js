// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

//Search button show/hide
function showSearch(){
    var div = document.getElementById("search");
    if (div.style.display === "none") {
        //$(div).fadeIn(500, 0).slideIn("display","");
        $(div).slideDown(500, function(){
            $(this).display = "block";
        });

        //Hide searchBtn
        var div = document.getElementById("searchBtn");
        div.style.display = "none";
    } else {
        //$(div).fadeOut().css("none","");
        $(div).fadeTo(500, 0).slideUp(500, function(){
            $(this).remove();
        });
    }
 }

 //Fadeout alert message
 window.setTimeout(function() {
    $(".alert").fadeTo(500, 0).slideUp(500, function(){
        $(this).remove(); 
    });
}, 2000);