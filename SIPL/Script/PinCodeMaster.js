$(document).ready(function () {
    Clear();

    //For btn save  click
    $("#btnPinCodeSave").click(function () {
        InsertUpdatePinCodeMaster();
    })
    //For Btn reset
    $("#PinCodeReset").click(function () {

        window.location.reload();
    })
    $("#ExportPinCodeExcel").click(function () {
        ExportToExcel();
    });
    
})




function InsertUpdatePinCodeMaster() {
    try {

        
        $.post("/Master/InsertUpdatePinCodeMaster",
            {
                PinCodeID: $("#txtPinCodeID").val().trim(),
                CountryCode: $("#txtCountryCode").val().trim(),
                StateCode: $("#txtState").val().trim(),
                City: $("#txtCityName").val().trim(),
                PinCode: $("#txtPinCode").val().trim(),
                Active: $("#PinCodeActive").is(":checked").toString(),

            },
            function (data) {
                alert(data.Message)
                if (data.Focus != "") {
                    $("#" + data.Focus).focus();
                }
                if (data.Status == "1") {
                    Clear();
                }

            })

    } catch (ex) {
        alert("Error in InsertUpdateCityMaster" + ex.message);
    }
}
function ShowPinCodeMaster() {
    try {
        $.post("/Master/ShowPinCodeMaster", { EditFunctionName: "EditPinCodeMaster", DeleteFunctionName: "DeletePinCodeMaster" }, function (data) {
            if (data.Message != "") {
                alert(data.Message);
            }
            if (data.Grid != "") {
                $("#dvPinCodeGrid").html(data.Grid);
            }
        })
    } catch (ex) {
        alert("Error in ShowCityMaster" + ex.message);
    }
}
function EditPinCodeMaster(PinCodeID) {
    $("#txtCountryCode").prop("readonly", true);
    $("#txtState").prop("readonly", true);
    $("#txtCityName").prop("readonly", true);
    try {
        $.post("/Master/EditPinCodeMaster", { PinCodeID: PinCodeID }, function (data) {
            $("#txtPinCodeID").val(data.PinCodeID);
            $("#txtCountryCode").val(data.Country);
            $("#txtState").val(data.State);
            $("#txtCityName").val(data.City);
            $("#txtPinCode").val(data.PinCode);
            $("#txtPinCode").focus();
            $("#PinCodeActive").prop("checked", data.Active === "Yes");
            $("#btnPinCodeSave").html('<i class="fas fa-pen-to-square"></i> Update');      
        })
    } catch (ex) {
        alert(ex.message);
    }
}

function DeletePinCodeMaster(PinCodeID) {
    try {
        if (confirm("Do you want to delete?")) {
            $.post("/Master/DeletePinCodeMaster", { PinCodeID: PinCodeID }, function (data) {
                if (data.Message != "") {
                    alert(data.Message);
                    Clear();
                }
            });
        }
    } catch (e) {
        alert("Error in DeleteStateMaster: " + e.message);
    }
}
function ExportToExcel() {
    window.location = "/Master/ExportToExcelPinCodeMaster";

}
function Clear() {
    $("#txtCountryCode").focus();
    $("#txtPinCodeID").val("");
    $("#txtCountryCode").val("");
    $("#txtState").val("");
    $("#txtCityName").val("");
    $("#txtPinCode").val("");
    $("#txtCountryCode").prop("readonly", false);
    $("#txtState").prop("readonly", false);
    $("#txtCityName").prop("readonly", false);
    $("#PinCodeActive").prop("checked", false);
    $("#btnPinCodeSave").html('<i class="fas fa-floppy-disk"></i> Save');
    ShowPinCodeMaster();
}