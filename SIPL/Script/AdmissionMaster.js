$(document).ready(function () {
    Clear();
    $("#Register").click(function () {
        InsertUpdateAdmissionForm();
    })
    $("#CourseSave").click(function () {
        SaveAcademicQualification();
    })
    $("#CourseReset").click(function () {
        ResetCourse();
    })
    $("#Reset").click(function () {
        ResetForm();
    })
})
function Year() {
    var Year = new Date().getFullYear();
    for (var i = Year; i > Year - 25; i--) {
        $("#Year").append(`<option value="${i}">${i}</option>`)
    }
}
function percentage() {
    var TotalMarks = $("#TotalMarks").val();
    var ObtainedMarks = $("#ObtainedMarks").val();

    if (TotalMarks == "" || ObtainedMarks == "") {
        $("#Percentage").val(0);
    }
    else {
        var Percentage = (ObtainedMarks * 100) / TotalMarks;
        $("#Percentage").val(Percentage.toFixed(2));
    }
}

function InsertUpdateAdmissionForm() {
    try {
        var StudentPhotoFile = $("#StudentPhoto")[0].files[0];

        if (StudentPhotoFile) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var base64Photo = e.target.result.split(',')[1];
                var FileName = StudentPhotoFile.name.split('.');

                $.ajax({
                    url: "/Master/InsertUpdateAdmissionForm",
                    type: "POST",
                    data: {
                        AdmissionNo: $("#AdmissionNo").val(), RegistrationNo: $("#RegistrationNo").val(),
                        StudentName: $("#StudentName").val(), ParentsName: $("#ParentsName").val(), ParentsProfession: $("#ParentsProfession").val(), StudentDob: $("#StudentDob").val(),
                        MobileNo: $("#MobileNo").val(), EmailId: $("#EmailId").val(), Gender: $("input[name='Gender']:checked").val(),
                        StudentPhoto: base64Photo, FileName: FileName[0], FileType: FileName[1], City: $("#City").val(), Address: $("#Address").val()
                    },
                    success: function (data) {
                        if (data.Message != "") {
                            alert(data.Message);
                        }
                        if (data.Focus != "") {
                            $("#" + data.Focus).focus();
                        }
                        if (data.Status == "1") {
                            ResetForm();
                        }

                    }
                });

            };
            reader.readAsDataURL(StudentPhotoFile);
        }
        else {
            $.ajax({
                url: "/Master/InsertUpdateAdmissionForm",
                type: "POST",
                data: {
                    AdmissionNo: $("#AdmissionNo").val(), RegistrationNo: $("#RegistrationNo").val(), StudentName: $("#StudentName").val(),
                    parentsName: $("#ParentsName").val(), ParentsProfession: $("#ParentsProfession").val(), StudentDob: $("#StudentDob").val(),
                    MobileNo: $("#MobileNo").val(), EmailId: $("#EmailId").val(),Gender: $("input[name='Gender']:checked").val(),
                    StudentPhoto: null, City: $("#City").val(), Address: $("#Address").val()
                },

                success: function (data) {
                    if (data.Message != "") {
                        alert(data.Message);
                    }
                    if (data.Focus != "") {
                        $("#" + data.Focus).focus();
                    }
                    if (data.Status == "1") {
                        ResetForm();
                    }

                }
            });

        }

    }
    catch (ex) {
        alert(ex.message)
    }
}
function SaveAcademicQualification() {
    try {
        var TempId = $("#TempId").val();

        $.post("/Master/SaveAcademicQualification", {
            TempId: (!TempId || TempId == 0) ? Math.floor(Math.random() * 1000000) + 1 : TempId,
            InstituteName: $("#InstituteName").val().trim(),
            CourseName: $("#Course").val().trim(),
            Year: $("#Year").val().trim(),
            TotalMarks: $("#TotalMarks").val().trim(),
            ObtainedMarks: $("#ObtainedMarks").val().trim(),
            Percentage: $("#Percentage").val().trim(),

        }, function (data) {
            if (data.Message != "") {
                alert(data.Message);
                $("#" + data.Focus).focus();
            }
            if (data.Status == "1") {
                ResetCourse()
            }
            ShowAcademicQualification();
        });
    }
    catch (ex) {
        alert("Error In Save AcademicQualificatin" +ex.Message);
    }
}

function ShowAcademicQualification() {
    try {
        $.post("/Master/ShowAcademicQualification",
            function (data) {
                $("#CourseDisplay").html(data.Grid)
            })
    }
    catch (ex) {
        alert(ex.Message);
    }
}
function EditAcademicQualification(id) {
    try {
        $.post("/Master/EditAcademicQualification", { TempId: id },
            function (data) {
                console.log(data.SelectedCourse);
                if (data.Message != "") {
                    alert(data.Message);
                }
                else {
                    $("#TempId").val(data.SelectedCourse.TempId);
                    $("#InstituteName").val(data.SelectedCourse.InstituteName);
                    $("#Course").val(data.SelectedCourse.CourseName);
                    $("#TotalMarks").val(data.SelectedCourse.TotalMarks);
                    $("#ObtainedMarks").val(data.SelectedCourse.ObtainedMarks);
                    $("#Percentage").val(data.SelectedCourse.Percentage);
                    $("#Year").val(data.SelectedCourse.Year);
                }
            })
    }
    catch (ex) {
        alert(ex.Message);
    }
}
function DeleteAcademicQualification(id) {
    try {
        if (confirm("Are you sure you want to delete this course?")) {
            $.post("/Master/DeleteAcademicQualification", { TempId: id },
                function (data) {
                    if (data.Messages != "") {
                        alert(data.Messages);
                    }
                    ShowAcademicQualification();
                })
        }
    }
    catch (ex) {
        alert(ex.Message);
    }
}

function ResetCourse() {
    try {
        $("#InstituteName").focus();
        $("#Year").empty();
        var Year = new Date().getFullYear();
        for (var i = Year; i > Year - 25; i--) {
            $("#Year").append(`<option value="${i}">${i}</option>`)
        }
        $("#TempId").val('');
        $("#InstituteName").val('');
        $("#Course").val('');
        $("#TotalMarks").val('');
        $("#ObtainedMarks").val('');
        $("#Year").val(Year);
        $("#Percentage").val('');
    }
    catch (ex) {
        alert(ex.Message);
    }
}
function StudentDob() {
    try {
        const dobInput = document.getElementById("StudentDob");
        if (dobInput) {
            dobInput.addEventListener("input", function () {
                // Allow only format yyyy-mm-dd, limit year to 4 digits
                this.value = this.value.replace(/^(\d{4})\d+/, '$1');
            });
        }
    } catch (ex) {
        alert(ex.Message);

    }
}
function Clear()
{
    $("#RegistrationNo").focus()
    Year();
    StudentDob();
}
function ResetForm() {
    try {
        window.location.reload()
        Clear();
    }
    catch (ex) {
        alert(ex.Message);
    }
}
function ShowStudentDetailFromRegistrationNo() {
    $.post("/Master/ShowStudentDetailFromRegistrationNo", { RegistrationNo: $("#RegistrationNo").val() },
        function (data) {    
                $("#StudentName").val(data.StudentName),
                $("#ParentsName").val(data.FatherName),
                $("#StudentDob").val(data.DateOfBirth);
                $("#MobileNo").val(data.MobileNo),
                $("#EmailId").val(data.EmailId),
                $("input[name='Gender'][value='" + data.Gender + "']").prop("checked", true);
                $("#City").val(data.City),
                $("#Address").val(data.Address)
            
            //$("#FileName").text(data.FileName);
        }
    )
}
function UpdateCheckBox() {
    try {
        if ($("#Update").is(":checked")) {

            $("#AdmissionNo").prop("disabled", false).focus();
            $("#AdmissionNo").removeAttr("Placeholder");

            $("#RegistrationNo").prop("disabled", true);
            $("#Register").text("Update")
           
            $("#FileName").text("");

            $("#Form").find("input, textarea, select").each(function () {
                var $el = $(this);
                if ($el.attr("id") === "Update") return;
                if ($el.is("input[type='checkbox'], input[type='radio']")) {
                    $el.prop("checked", false); 
                } else if ($el.is("input[type='file']")) {
                    $el.val(""); 
                } else {
                    $el.val(""); 
                }
            });
            $("#Form").find("img").each(function () {
                $(this).attr("src", "").hide();
            });
        } else {

            $("#AdmissionNo").prop("disabled", true).val('').attr("placeholder", "System Generated");

            $("#RegistrationNo").prop("disabled", false);
            window.location.reload();
        }
    } catch (ex) {
        alert("Error in UpdateCheckBox: " + ex.message);
    }
}
function ShowStudentDetailFromAdmission() {
    $.post("/Master/EditAdmssionMaster", { AdmissionNo: $("#AdmissionNo").val() },
        function (data) {
            $("#RegistrationNo").val(data.RegistrationNo),
                $("#StudentName").val(data.StudentName),
                $("#ParentsName").val(data.ParentsName),
                $("#ParentsProfession").val(data.ParentsProfession),
                $("#StudentDob").val(data.StudentDob),
                $("#EmailId").val(data.EmailId),
                $("#MobileNo").val(data.MobileNo),
                $("input[name='Gender'][value='" + data.Gender + "']").prop("checked", true)
                $("#City").val(data.City),
                $("#Address").val(data.Address)
                if (data.StudentPhoto != null && data.StudentPhoto != "") {
                    $("#StudentPhotoPreview").attr("src", "data:image/" + data.FileType + ";base64," + data.StudentPhoto).show();

                }
            $("#FileName").text(data.FileName);
                
                ShowAcademicQualification();
        }
    )
}
