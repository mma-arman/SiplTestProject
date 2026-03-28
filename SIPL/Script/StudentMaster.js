$(document).ready(function () {
    DateOfBirthValidation();
    ResetCourse();
    PasswordShowHide();
    $("#StudentName").focus();
    $("#Register").click(function () {
     InsertUpdateStudent();
    })
    $("#CourseSave").click(function () {
        SaveCourseInSession();
    })
      $("#CourseReset").click(function () {
        ResetCourse();
      })
    $("#Reset").click(function () {
        ResetForm();
    })
})



function InsertUpdateStudent() {
    try {
        var StudentPhotoFile = $("#StudentPhoto")[0].files[0];

        if (StudentPhotoFile) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var base64Photo = e.target.result.split(',')[1];
                var FileName = StudentPhotoFile.name.split('.');
                
                $.ajax({
                    url: "/Master/InsertUpdateStudentMaster",
                    type: "POST",
                    data: {
                        RegistrationNo: $("#RegistrationNo").val(), StudentName: $("#StudentName").val(), FatherName: $("#FatherName").val(), DateOfBirth: $("#DateOfBirth").val(),
                        MobileNo: $("#MobileNo").val(), EmailId: $("#EmailId").val(), Password: $("#Password").val(), Gender: $("input[name='Gender']:checked").val(),
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
                url: "/Master/InsertUpdateStudentMaster",
                type: "POST",
                data: {
                    RegistrationNo: $("#RegistrationNo").val() ,StudentName: $("#StudentName").val(), FatherName: $("#FatherName").val(), DateOfBirth: $("#DateOfBirth").val(),
                    MobileNo: $("#MobileNo").val(), EmailId: $("#EmailId").val(), Password: $("#Password").val(), Gender: $("input[name='Gender']:checked").val(),
                    StudentPhoto: null,  City: $("#City").val(), Address: $("#Address").val()
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
    catch(ex) {
        alert(ex.message)
    }
}
function SaveCourseInSession() {
    try {
        var TempId = $("#TempId").val();

        $.post("/Master/SaveCourse", {
            TempId: (!TempId || TempId == 0) ? Math.floor(Math.random() * 1000000) + 1 : TempId,
            CourseName: $("#Course").val().trim(),
            TotalMarks: $("#TotalMarks").val().trim(),
            ObtainedMarks: $("#ObtainedMarks").val().trim(),
            Year: $("#Year").val().trim()
        }, function (data) {
            if (data.Message != "") {
                alert(data.Message);
                $("#" + data.Focus).focus();
            }
            if (data.Status=="1") {
                ResetCourse()
            }
            ShowCourse();
        });
    }
    catch (ex) {
        alert(ex.Message);
    }
}
function ShowCourse() {
    try {
        $.post("/Master/ShowCourse",
            function (data) {
                $("#CourseDisplay").html(data.Grid)
            })
    }
    catch (ex) {
        alert(ex.Message);
    }
}
function EditCourse(id)
{
    try {
        $.post("/Master/EditCourse", { TempId: id },
            function (data) {
                console.log(data.SelectedCourse);
                if (data.Message != "") {
                    alert(data.Message);
                }
                else {
                    $("#TempId").val(data.SelectedCourse.TempId);
                    $("#Course").val(data.SelectedCourse.CourseName);
                    $("#TotalMarks").val(data.SelectedCourse.TotalMarks);
                    $("#ObtainedMarks").val(data.SelectedCourse.ObtainedMarks);
                    $("#Year").val(data.SelectedCourse.Year);

                }
            })
    }
    catch (ex) {
        alert(ex.Message);
    }
}
function DeleteCourse(id) {
    try {
        if (confirm("Are you sure you want to delete this course?")) {
            $.post("/Master/DeleteCourse", { TempId: id },
                function (data) {
                    if (data.Messages != "") {
                        alert(data.Messages);
                    }
                    ShowCourse();
                })
        }
    }
    catch (ex) {
        alert(ex.Message);
    }
}
function ResetCourse() {
    try {
        $("#Course").focus();
        $("#Year").empty();
        var Year = new Date().getFullYear();
        for (var i = Year; i > Year - 25; i--) {
            $("#Year").append(`<option value="${i}">${i}</option>`)
        }
        $("#TempId").val('');
        $("#Course").val('');
        $("#TotalMarks").val('');
        $("#ObtainedMarks").val('');
        $("#Year").val(Year);
    }
    catch (ex) {
        alert(ex.Message);
    }
}
function ResetForm() {
    try {
        window.location.reload()
    }
    catch (ex) {
        alert(ex.Message);
    }
}

function DateOfBirthValidation() {
    try {
        const dobInput = document.getElementById("DateOfBirth");
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

function PasswordShowHide() {
    try {
        $("#togglePassword").hover(
            function () {
                $("#Password").attr("type", "text");
                $("#togglePassword i").removeClass("fa-eye-slash").addClass("fa-eye");
            },
            function () {
                $("#Password").attr("type", "password");
                $("#togglePassword i").removeClass("fa-eye").addClass("fa-eye-slash");
            }
        );
    } catch (ex) {
        alert(ex.Message);
    }
}
function UpdateCheckBox() {
    try {
     
        if ($("#Update").is(":checked")) {
           
            $("#RegistrationNo").removeAttr("disabled");
            $("#RegistrationNo").removeAttr("placeholder");
            $("#RegistrationNo").focus();
            $("#Register").text("Update");

       
        }
        else {
            $("#RegistrationNo").attr("disabled", true);
            $("#RegistrationNo").attr("placeholder", "System Generated");
            window.location.reload();
        }
    } catch (ex) {
        alert(ex.Message);
    }
    
}
function EditStudentMaster() {
    try {

            $.post("/Master/EditStudentMaster", { RegistrationNo: $("#RegistrationNo").val() },
                function (data) {
                    $("#StudentName").val(data.StudentName),
                    $("#FatherName").val(data.FatherName),
                    $("#DateOfBirth").val(data.DateOfBirth),
                    $("#MobileNo").val(data.MobileNo),
                    $("#EmailId").val(data.EmailId),
                    $("#Password").val(data.Password),
                    $("input[name='Gender'][value='" + data.Gender + "']").prop("checked", true);
                    $("#City").val(data.City),
                    $("#Address").val(data.Address)
                    if (data.StudentPhoto != null && data.StudentPhoto != "")
                    {
                        $("#StudentPhotoPreview").attr("src", "data:image/" + data.FileType + ";base64," + data.StudentPhoto).show();;
                    
                    }
                    $("#FileName").text(data.FileName);
                    ShowCourse();


                }
            )
        
    }
    catch (ex) {
        alert("Error in EditStudentMaster");
    }
}