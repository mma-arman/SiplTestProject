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
    $.post("/Master/AdmissionReport", {
        AdmissionNo: $("#AdmissionNo").val().trim(), RegistrationNo: $("#RegistrationNo").val().trim(),
        StudentName: $("#StudentName").val().trim(),
        ParentsName: $("#ParentsName").val().trim(), MobileNo: $("#MobileNo").val().trim(), Gender: $("#Gender").val(),
        AdmDateFrom: $("#AdmDateFrom").val(), AdmDateTo: $("#AdmDateTo").val()
    }, function (data) {
        if (data.Message != "") {
            alert(data.Message)
        }

        $("#dvGrid").html(data.Grid);

        $("#Msg").html(data.DataMsg);

    })
}
function ExportSearchedStudent() {
    window.location = "/Master/ExportSearchedStudentOnAdmission";
}
function RegistrationDate() {
    var Today = new Date();
    var Past = new Date();
    Past.setDate(Today.getDate() - 30);
    $("#AdmDateFrom").val(Past.toISOString().split('T')[0]);
    $("#AdmDateTo").val(Today.toISOString().split('T')[0]);
    $("#AdmissionNo").focus();
}
function PrintReport(AdmissionNo) {
    window.location.href = "/AdmissionReport.aspx?AdmissionNo=" + AdmissionNo;
}