var StateID = 0;

$(document).ready(function () {
    Clear();
    // Save button click
    $("#btnStateSave").click(function () {
        InsertUpdateStateMaster();
        return false;
    });

    // Reset button click
    $("#StateReset").click(function () {
        window.location.reload();
    });

    //Export Function Call
    $("#ExportExcel").click(function () {
        ExportToExcel();
    });
});

function InsertUpdateStateMaster() {
    try {
        $.post(
            "InsertUpdateStateMaster",
            {
                StateID: StateID,
                CountryCode: $("#txtCountryCode").val(),
                StateCode: $("#txtStateCode").val(),
                StateName: $("#txtStateName").val(),
                Active: $("#StateActive").is(":checked").toString(),
            },
            function (data) {
                alert(data.Message);
                if (data.Focus != "") {
                    $("#" + data.Focus).focus();                
                }
                if(data.Status=="1") {
                    Clear();
                }
            }
        );
    } catch (e) {
        alert("Error in InsertUpdateStateMaster: " + e.message);
    }
}
function ShowStateMaster() {
    try {
        $.post("/Master/ShowStateMaster", { Proc_name: "USP_ShowStateMaster", EditFunctionName: "EditStateMaster", DeleteFunctionName:"DeleteStateMaster" }, function (data) {
            if (data.Message != "") {
                alert(data.Message);
            }
            if (data.Grid != "") {
                $("#dvStateGrid").html(data.Grid);
            }
        });
    } catch (e) {
        alert("Error in ShowStateMaster: " + e.message);
    }
}

function EditStateMaster(ID) {
    $("#txtCountryCode").prop("readonly", true);
    $("#txtStateCode").prop("readonly", true);
    $("#txtStateName").focus();
    try {
        StateID = ID;
        $.post("EditStateMaster", { StateID: StateID },
            function (data) {
            $("#txtCountryCode").val(data.CountryCode);
            $("#txtStateCode").val(data.StateCode);
            $("#txtStateName").val(data.StateName);
            $("#StateActive").prop("checked", data.Active === "True");
            $("#btnStateSave").html('<i class="fas fa-pen-to-square"></i> Update');
        });
    } catch (e) {
        alert("Error in EditStateMaster: " + e.message);
    }
}
function DeleteStateMaster(ID) {
    try {
        if (confirm("Do you want to delete?")) {
            $.post("DeleteStateMaster", { StateID: ID }, function (data) {
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
    window.location = "/Master/ExportToExcelStateMaster";

}
function Clear() {
 
    $("#txtCountryCode").val("");
    $("#txtStateCode").val("");
    $("#txtStateName").val("");
    $("#txtCountryCode").focus();
    $("#txtCountryCode").prop("readonly", false);
    $("#txtStateCode").prop("readonly", false);
    $("#StateActive").prop("checked", false);
    $("#btnStateSave").html('<i class="fas fa-floppy-disk"></i> Save');
    ShowStateMaster();
}
