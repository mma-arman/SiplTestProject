$(document).ready(function () {
    Clear()
    $("#Save").click(function () {
        InsertUpdateUser();
    })
    $("#Reset").click(function () {
        window.location.reload();
    })
    $("#Export").click(function () {
        ExportToExcel();
    })
})
function InsertUpdateUser() {
    try {
        $.post("/Master/InsertUpdateUserMaster", {
            UserId: $("#UserId").val()?.trim(),
            UserName: $("#UserName").val()?.trim(),
            MobileNo: $("#MobileNo").val()?.trim(),
            EmailId: $("#EmailId").val()?.trim(),
            Password: $("#Password").val()?.trim(),
            Address: $("#Address").val()?.trim(),
            Active: $("#Active").is(":checked").toString(),
        },
            function (data) {               
                alert(data.Message);
                if (data.Focus != "") {
                    $("#" + data.Focus).focus();
                }
                if (data.Status=="1") {
                    Clear();
                }
        })
    }
    catch (ex) {
        alert(ex.message);
    }
}

function ShowUserMaster() {
    try {

        $.post("/Master/ShowUserMaster", { EditFunctionName: "EditUserMaster", DeleteFunctionName: "DeleteUserMaster" },
            function (data) {
                if (data.Message != "") {
                    alert(data.Message)
                }
                if (data.Grid != "") {
                    $("#dvGrid").html(data.Grid);
                }
            })

    } catch (e) {
        alert("Error in show User Master : " + e.message);
    }
}
function EditUserMaster(ID) {
   
    try {
        $.post("/Master/EditUserMaster", { UserId: ID },
            function (data) {
                if (data.Message != "") {
                    alert(data.Message)
                }
                else {
                    UserId = ID;
                    $("#UserName").val(data.UserName),
                        $("#UserId").val(data.UserId),
                        $("#UserCode").val(data.UserCode),
                        $("#MobileNo").val(data.MobileNo),
                        $("#EmailId").val(data.EmailId),
                        $("#Password").prop("disabled", true)
                        $("#Password").val("********")
                        $("#Address").val(data.Address),
                    $("#Active").prop("checked", data.Active === "Yes");
                    $("#Save").html('<i class="fas fa-pen-to-square"></i> Update');
                }
            })

    } catch (e) {
        alert("Error in EditCountryMaster : " + e.message);
    }
}
function DeleteUserMaster(ID) {
    try {
        if (confirm("do you want to delete?")) {


            $.post("/Master/DeleteUserMaster", { UserId: ID },
                function (data) {
                    if (data.Message != "") {
                        alert(data.Message)
                        Clear()
                    }
                })
        }
    } catch (e) {
        alert("Error in DeleteUserMaster : " + e.message);
    }
}
function ExportToExcel() {
    window.location = "/Master/ExportToExcelUserMaster";

}
function Clear() {
    ShowUserMaster();
    $("#UserName").focus();
    $("#Password").prop("disabled", false)
    $("#UserName").val(""),
    $("#UserId").val(""),
    $("#UserCode").val("System Generated"),
    $("#MobileNo").val(""),
    $("#EmailId").val(""),
    $("#Password").val(""),
        $("#Address").val("")
    $("#Active").prop("checked", false);
    $("#Save").html('<i class="fas fa-save-to-square"></i> Save');
}