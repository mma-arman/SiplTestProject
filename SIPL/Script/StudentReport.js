$(document).ready(function () {
    $("#Search").click(function () {
        SearchStudent();
    })
    $("#Export").click(function () {
        ExportSearchedStudent();
    })
    $("#Reset").click(function () {
        window.location.reload();
    })
    RegistrationDate()
})
function SearchStudent() {
    $.post("/Master/SearchStudent", {
        RegistrationNo: $("#RegistrationNo").val().trim(), StudentName: $("#StudentName").val().trim(),
        FatherName: $("#FatherName").val().trim(), MobileNo: $("#MobileNo").val().trim(), Gender: $("#Gender").val(),
        RegDateFrom: $("#RegDateFrom").val(), RegDateTo: $("#RegDateTo").val()
    }, function (data) {
        if (data.Message != "") {
            alert(data.Message)
        }
 
            $("#dvGrid").html(data.Grid);

            $("#Msg").html(data.DataMsg);

    })
}
function ExportSearchedStudent() {
    window.location = "/Master/ExportSearchedStudent";
}
function RegistrationDate() {
    var Today = new Date();
    var Past = new Date();
    Past.setDate(Today.getDate() - 30);
    $("#RegDateFrom").val(Past.toISOString().split('T')[0]);
    $("#RegDateTo").val(Today.toISOString().split('T')[0]);
    $("#RegistrationNo").focus();
}
function PrintReport(RegistrationNo) {
    window.location.href = "/StudentReport.aspx?RegistrationNo=" + RegistrationNo;
}