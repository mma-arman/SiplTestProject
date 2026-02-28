$(document).ready(function () {

    $("#btnCitySave").click(function () {
        InsertUpdateCityMaster();
    })
    $("#Reset").click(function () {
        window.location.reload();
    });
    Clear();
    //Export Function Call
    $("#ExportExcel").click(function () {
        ExportToExcel();
    });
})
function InsertUpdateCityMaster() {
    try {



        $.post("/master/InsertUpdateCityMaster",
            {
                CityID: $("#txtCityID").val().trim(),
                Country: $("#txtCountryCode").val().trim(),
                State: $("#txtState").val().trim(),
                City: $("#txtCityName").val().trim(),
                Active: $("#CityActive").is(":checked").toString(),

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
        alert( "Error in InsertUpdateCityMaster" + ex.message);
    }
}

function ShowCityMaster() {
    try {
        $.post("/Master/ShowCityMaster", { Proc_name: "USP_ShowCityMaster", EditFunctionName: "EditCityMaster", DeleteFunctionName: "DeleteCityMaster" }, function (data) {
            if (data.Message != "") {
                alert(data.Message);
            }
            if (data.Grid != "") {
                $("#dvCityGrid").html(data.Grid);
            }
        })
    } catch (ex) {
        alert("Error in ShowCityMaster" + ex.message);
    }  
}
function EditCityMaster(CityID) {
    $("#txtCountryCode").prop("readonly", true);
    $("#txtState").prop("readonly", true);
    $("#txtCityName").focus();
    try {
        $.post("/Master/EditCityMaster", { CityID:CityID }, function (data) {
            $("#txtCityID").val(data.CityID);
            $("#txtCountryCode").val(data.Country);
            $("#txtState").val(data.State);
            $("#txtCityName").val(data.City);
            $("#btnCitySave").html('<i class="fas fa-pen-to-square"></i> Update');
            $("#CityActive").prop("checked", data.Active === "Yes");
        })
    } catch (ex) {
        alert(ex.message);
    }

    
}
function DeleteCityMaster(ID) {
    try {
        if (confirm("Do you want to delete?")) {
            $.post("/Master/DeleteCityMaster", { CityID: ID }, function (data) {
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
    window.location = "/Master/ExportToExcelCityMaster";

}
function Clear() {
    $("#txtCountryCode").val("");
    $("#txtState").val("")
    $("#txtCityName").val("")
    $("#txtCountryCode").focus();
    ShowCityMaster();
    $("#CityActive").prop("checked", false);
    $("#txtCountryCode").prop("readonly", false);
    $("#txtState").prop("readonly", false);
    $("#btnCitySave").html('<i class="fas fa-floppy-disk"></i> Save');
}