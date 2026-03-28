$(document).ready(function ()
{
    $("#Submit").click(function () {
        login()
    })

});
function login() {
    try {
        $.post("/master/login",
            {
                UserCode: $("#UserCode").val()?.trim(),
                Password: $("#Password").val()?.trim()
            }, function (data) {
                if (data.Status == "1") {
                    window.location.href = '/Master/WelcomePage'
                }
                else {
                    if (data.Message != "") {
                        alert(data.Message);
                    }
                }
               
                
            })
    }
    catch (ex) {
        alert(ex.message);
    }
    
}