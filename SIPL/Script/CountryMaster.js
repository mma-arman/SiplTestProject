var CountryID = 0;
$(document).ready(function () {
   
    ClearD();
    $("#btnSave").click(function () {
        InsertUpdateCountryMaster();
      
    });
    $("#btnReset").click(function () {
        window.location.reload();
        
    });

    //Export Function Call
    $("#ExportExcel").click(function () {
        ExportToExcel();
    });
});
function InsertUpdateCountryMaster() {
   try {
       $.post("/Master/InsertUpdateCountryMaster",
           {
               CountryID: CountryID,
               CountryCode: $("#txtCountryCode").val(),
               CountryName: $("#txtCountryName").val(),
               CountryActive: $("#CountryActive").is(":checked").toString(),
           },
           function (data) {
               alert(data.Message);
               if (data.Focus != "") {
                   $("#" + data.Focus).focus();
               }
               if (data.Status == "1") {
                   ClearD();
               }
           }
       );

    } catch (e) {
        alert("Error in InsertUpdateCountryMaster :" + e.message);
    }
};
function ShowCountryMaster() {
    try {
    
        $.post("/Master/ShowCountryMaster", { Proc_name: "USP_ShowCountryMaster", EditFunctionName: "EditCountryMaster", DeleteFunctionName:"DeleteCountryMaster" },
            function (data) {
                if (data.Message != "") {
                    alert(data.Message)
                } 
                if (data.Grid!="") {
                    $("#dvGrid").html(data.Grid);
                }
            })

    } catch (e) {
        alert("Error in show CountryMAster : " +e.message);
    }
}
function EditCountryMaster(ID) {
    $("#txtCountryCode").prop("readonly", true)
    try {
        $.post("/Master/EditCountryMaster", { CountryID: ID },
            function (data) {
                if (data.Message != "") {
                    alert(data.Message)
                }
                else {
                    CountryID = ID;
                    $("#txtCountryName").val(data.CountryName),
                    $("#txtCountryCode").val(data.CountryCode)
                    $("#CountryActive").prop("checked", data.CountryActive === "True");
                    $("#btnSave").html('<i class="fas fa-pen-to-square"></i> Update');
                }
            })

    } catch (e) {
        alert("Error in EditCountryMaster : " + e.message);
    }
}
function DeleteCountryMaster(ID) {
    try {
        if (confirm("do you want to delete?")) {


            $.post("/Master/DeleteCountryMaster", { CountryID: ID },
                function (data) {
                    if (data.Message != "") {
                        alert(data.Message)
                        ClearD()
                    }
                })
        }
    } catch (e) {
        alert("Error in DeleteCountryMaster : " + e.message);
    }
}
function ExportToExcel() {
    window.location = '/Master/ExportToExcelCountryMaster';
};

function ClearD() {
    CountryID = 0;
    $("#txtCountryCode").val("");
    $("#txtCountryName").val("");
    $("#txtCountryCode").focus();
    $("#CountryActive").prop("checked", false);
    $("#btnSave").html('<i class="fas fa-floppy-disk"></i> Save');
    ShowCountryMaster();

}