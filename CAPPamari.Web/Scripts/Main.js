var editMode = '';
var viewModel = null;
var alertDialog = null;
var draggedCourse = null;

$(window).resize(function () {
    ResizeDisplay();
});
$(window).load(function () {
    viewModel = new ViewModel();
    if (LoadUserFromCookie()) {
        viewModel.loadCAPPReport();
    }

    if (!window.File || !window.FileReader) {
        HideCsvImportAbility();
    }

    ko.applyBindings(viewModel);

    SetupAlertDialog();
    SetupDragAndDrop();
    ResizeDisplay();
});

Alert = function (text) {
    alertDialog.dialog({
        title: text
    })
    alertDialog.dialog('open');
    alertDialog.hide();
}
SetupAlertDialog = function (advisor) {
    alertDialog = $('#alertDialog');
    alertDialog.dialog({
        autoOpen: false,
        resizable: false,
        draggable: false,
        hide: 'fade',
        position: 'bottom',
        minHeight: 0,
        open: function () {
            setTimeout(function () {
                alertDialog.dialog('close');
            }, 5000);
        }
    });
}
AutopopulateUnappliedCourses = function () {
    $.ajax({
        url: window.location.origin + '/api/Course/AutopopulateUnappliedCourses',
        data: JSON.stringify(viewModel.user().userName()),
        type: 'POST',
        contentType: 'application/json',
        success: function (data, textStatus, jqXHR) {
            if (!data.Success) {
                Alert(data.Message);
            }
            viewModel.setCAPPReport(data.Payload);
            $('#blockingDiv').hide();
        },
        error: function () {
            Alert('There is a problem with the server.  Please try again later');
            $('#blockingDiv').hide();
        }
    });
    $('#blockingDivSpan').text('Auto-populating courses...');
    $('#blockingDiv').show();
}
HideCsvImportAbility = function () {
    var importTable = $('#singletonClassAddDialogRoot').find('table');
    importTable.find('tr:nth-child(9n)').hide();
    importTable.find('tr:nth-child(10n)').hide();
    importTable.find('tr:nth-child(11n)').hide();
    importTable.find('tr:nth-child(12n)').hide();
}
EmailToAdvisor = function (advisor) {
    var emailRequest = {
        UserName: viewModel.user().userName(), Advisor: {
            Name: advisor.name(),
            EMail: advisor.emailAddress()
        }
    };
    $.ajax({
        url: window.location.origin + '/api/User/EmailToAdvisor',
        data: JSON.stringify(emailRequest),
        type: 'POST',
        contentType: 'application/json',
        success: function (data, textSuccess, jqXHR) {
            Alert(data.Message);
            $('#blockingDiv').hide();
        },
        error: function () {
            Alert('There is an issue with the server, please try again later');
            $('#blockingDiv').hide();
        }
    });
    $('#blockingDivSpan').text('Sending email...');
    $('#blockingDiv').show();
}
DeleteAdvisor = function (advisor) {
    var removeAdvisorRequest = { UserName: viewModel.user().userName(), NewAdvisor: { Name: advisor.name(), EMail: advisor.emailAddress() } };
    $.ajax({
        url: window.location.origin + '/api/User/RemoveAdvisor',
        data: JSON.stringify(removeAdvisorRequest),
        type: 'POST',
        contentType: 'application/json',
        success: function (data, textStatus, jqXHR) {
            if (!data.Success || !data.Payload) {
                Alert(data.Message);
                $('#blockingDiv').hide();
                return;
            }

            viewModel.user().advisors.pop(advisor);

            $('#blockingDiv').hide();
            RedisplayHeader();
        },
        error: function () {
            $('#blockingDiv').hide();
        }
    });
    $('#blockingDivSpan').text('Deleting advisor...');
    $('#blockingDiv').show();
}
EditAdvisorInfo = function (advisor) {
    editMode = 'edit';
    $('#advisorName').val(advisor.name());
    $('#advisorEmail').val(advisor.emailAddress());
    $('#advisorName').prop('readonly', 'true');
    ShowAdvisorDialog();
}
ShowAdvisorDialog = function () {
    $('#advisorDialogRoot').show();
}
AddNewAdvisor = function () {
    editMode = 'new';
    $('#registrationUserName').val('');
    $('#registrationMajor').val('');
    $('#registrationUserName').prop('readonly', 'false');
    ShowAdvisorDialog();
}
SubmitAdvisorInformation = function () {
    var name = $('#advisorName').val();
    var email = $('#advisorEmail').val();
    if (editMode === 'new') {
        var newAdvisorRequest = { UserName: viewModel.user().userName(), NewAdvisor: { Name: name, EMail: email }};
        $.ajax({
            url: window.location.origin + '/api/User/AddAdvisor',
            data: JSON.stringify(newAdvisorRequest),
            type: 'POST',
            contentType: 'application/json',
            success: function (data, textStatus, jqXHR) {
                if (!data.Success || !data.Payload) {
                    Alert(data.Message);
                    $('#blockingDiv').hide();
                    return;
                }

                viewModel.user().advisors.push(new Advisor(name, email));

                $('#blockingDiv').hide();
                RedisplayHeader();
            },
            error: function () {
                Alert('There is something wrong with the server, please try again later');
                $('#blockingDiv').hide();
            }
        });
        $('#blockingDivSpan').text('Adding advisor...');
        $('#blockingDiv').show();
    } else if (editMode === 'edit') {
        var editAdvisorRequest = { Name: name, EMail: email };
        $.ajax({
            url: window.location.origin + '/api/User/UpdateAdvisor',
            data: JSON.stringify(editAdvisorRequest),
            type: 'POST',
            contentType: 'application/json',
            success: function (data, textStatus, jqXHR) {
                if (!data.Success || !data.Payload) {
                    Alert(data.Message);
                    $('#blockingDiv').hide();
                    return;
                }

                ko.utils.arrayForEach(viewModel.user().advisors(), function (advisor) {
                    if (advisor.name() === name) {
                        advisor.emailAddress(email);
                    }
                });

                $('#blockingDiv').hide();
                RedisplayHeader();
            },
            error: function () {
                Alert('There is something wrong with the server, please try again later');
                $('#blockingDiv').hide();
            }
        });
        $('#blockingDivSpan').text('Updating advisor...');
        $('#blockingDiv').show();
    }
    $('#advisorDialogRoot').hide();
}
CancelAdvisorDialog = function () {
    $('#advisorDialogRoot').hide();
}
MakeCoursesDraggable = function () {
    $(".course").draggable({
        appendTo: "body",
        helper: "clone",
        revert: true,
        containment: "#mainScreen"
    });
}
SetupDragAndDrop = function () {
    $(".requirementBox").accordion({
        collapsible: true,
    });
    $(".requirementBox").droppable({
        drop: function (event, ui) {
            var course = $(event.srcElement).parent().find('.courseData').data('course');
            if(course === undefined) course = $(event.srcElement).find('.courseData').data('course'); 
            var requirementSetName = $(event.target).find('h3').find('a').data('reqsetname');
            var moveCourseRequest = {
                UserName: viewModel.user().userName(),
                CourseToMove: {
                    DepartmentCode: course.department,
                    CourseNumber: course.number,
                    Grade: course.grade,
                    Credits: course.credits,
                    Semester: course.semester,
                    PassNoCredit: course.passNoCredit,
                    CommIntensive: course.commIntensive 
                },
                RequirementSetName: requirementSetName
            };
            $.ajax({
                url: window.location.origin + '/api/Course/MoveCourse',
                data: JSON.stringify(moveCourseRequest),
                type: 'POST',
                contentType: 'application/json',
                success: function (data, textStatus, jqXHR) {
                    if (!data.Success || !data.Payload) {
                        Alert(data.Message);
                        return;
                    }
                    ui.draggable.appendTo($(event.target).find(".courses"));
                    ui.helper.remove();
                    $(".requirementBox").accordion("resize");
                    if ($(this).accordion("option", "active") === false)
                    {
                        $(this).accordion("option", "active", 0);
                    }
                    $('#blockingDiv').hide();
                },
                error: function () {
                    Alert('There is a problem with the server, please try again later');
                    $('#blockingDiv').hide();
                }
            });
            Alert('Moving course...');
        }
    });
    $("#sidebarWrapper #courses").droppable({
        drop: function (event, ui) {
            var course = $(event.srcElement).parent().find('.courseData').data('course');
            if(course === undefined) course = $(event.srcElement).find('.courseData').data('course'); 
            var moveCourseRequest = {
                UserName: viewModel.user().userName(),
                CourseToMove: {
                    DepartmentCode: course.department,
                    CourseNumber: course.number,
                    Grade: course.grade,
                    Credits: course.credits,
                    Semester: course.semester,
                    PassNoCredit: course.passNoCredit,
                    CommIntensive: course.commIntensive 
                },
                RequirementSetName: 'Unapplied Courses' 
            };
            $.ajax({
                url: window.location.origin + '/api/Course/MoveCourse',
                data: JSON.stringify(moveCourseRequest),
                type: 'POST',
                contentType: 'application/json',
                success: function (data, textStatus, jqXHR) {
                    if (!data.Success || !data.Payload) {
                        Alert(data.Message);
                        $('#blockingDiv').hide();
                        return;
                    }
                    ui.draggable.appendTo($('#courses'));
                    ui.helper.remove();
                    $(".requirementBox").accordion("resize");
                    $('#blockingDiv').hide();
                },
                error: function () {
                    Alert('There is a problem with the server, please try again later');
                    $('#blockingDiv').hide();
                }
            });
            $('#blockingDivSpan').text('Moving course...');
            $('#blockingDiv').show();
        }
    });
    MakeCoursesDraggable();
}
ImportCSVFile = function () {
    if (!window.File || !window.FileReader) {
        Alert('Your browser does not support file importing!');
        return;
    }

    var autopop = $('#csvAutoPopulateCheckbox')[0].checked;
    var fileToRead = $('#csvFileInput')[0].files[0];
    var reader = new FileReader();
    reader.readAsText(fileToRead);
    reader.onload = function (event) {
        var csvData = event.target.result;
        var csvImportRequest = { UserName: viewModel.user().userName(), CsvData: csvData, Autopopulate: autopop };
        $.ajax({
            url: window.location.origin + '/api/Course/AddCsvFile',
            data: JSON.stringify(csvImportRequest),
            type: 'POST',
            contentType: 'application/json',
            success: function (data, textStatus, jqXHR) {
                if (!data.Success) {
                    Alert(data.Message);
                }
                viewModel.setCAPPReport(data.Payload);
                $('#singletonClassAddDialogRoot').hide();
                $('#blockingDiv').hide();
            },
            error: function () {
                Alert('There is an error with the server.  Please try again later');
                $('#singletonClassAddDialogRoot').hide();
                $('#blockingDiv').hide();
            }
        });
        $('#blockingDivSpan').text('Uploading courses...');
        $('#blockingDiv').show();
    };
    reader.onerror = function () {
        Alert('Unable to read file');
        $('#singletonClassAddDialogRoot').hide();
        $('#blockingDiv').hide();
    };
    $('#blockingDivSpan').text('Reading in CSV...');
    $('#blockingDiv').show();
}
SubmitSingletonClassAddInformation = function () {
    var deptCode = $('#singletonDepartment').val();
    var courseNumber = $('#singletonCourseNumber').val();
    var semesterCode = $('#singletonSemesterCode').val();
    var passNoCredit = $('#singletonPassNoCredit')[0].checked;
    var commIntensive = $('#singletonCommIntensive')[0].checked;
    var grade = $('#singletonGrade').val();
    var credits = $('#singletonCredits').val();

    var errorMessage = '';
    // make sure deptCode is a 4 letter all caps code
    if(!deptCode.match(/^[A-Z]{4}$/)) {
        errorMessage += 'Please enter a valid Department Code\n';
    }
    if(!courseNumber.match(/^[1|2|4|6][x|0-9]{3}$/)) {
        errorMessage += 'Please enter a valid Course Number\n';
    }
    if (!semesterCode.match(/^[S|M|F][0-9]{2}$/)) {
        errorMessage += 'Please enter a valid Semester Code\n';
    }
    if (!grade.match(/^[0-3].[0-9]{2}|4.00$/)) {
        errorMessage += 'Please enter a valid Grade\n';
    }
    if (!credits.match(/^[0-4]$/)) {
        errorMessage += 'Please enter a valid Credit\n';
    }
    if (errorMessage !== '') {
        Alert(errorMessage);
        return;
    }

    var newCourseRequest = {
        UserName: viewModel.user().userName(), NewCourse: {
            DepartmentCode: deptCode,
            CourseNumber: courseNumber,
            Grade: grade,
            Credits: credits,
            Semester: semesterCode,
            PassNoCredit: passNoCredit,
            CommIntensive: commIntensive
        }
    };
    $.ajax({
        url: window.location.origin + '/api/Course/AddNewCourse',
        data: JSON.stringify(newCourseRequest),
        type: 'POST',
        contentType: 'application/json',
        success: function (data, textSuccess, jqXHR) {
            if (!data.Success) {
                Alert(data.Message);
                return;
            }

            var course = new Course(deptCode, courseNumber, semesterCode, passNoCredit, grade, credits, commIntensive);
            viewModel.addNewCouse(course);
            $('#blockingDiv').hide();
        },
        error: function () {
            Alert('There is an issue with the server, please try again later');
            $('#blockingDiv').hide();
        }
    });
    $('#blockingDivSpan').text('Adding your course...');
    $('#blockingDiv').show();
    $('#singletonClassAddDialogRoot').hide();
}
CancelSingletonClassAdd = function () {
    $('#singletonDepartment').val('');
    $('#singletonCourseNumber').val('');
    $('#singletonSemesterCode').val('');
    $('#singletonPassNoCredit')[0].checked = false;
    $('#singletonGrade').val('');
    $('#singletonCredits').val('');

    $('#singletonClassAddDialogRoot').hide();
}
ShowSingletonClassAddDialog = function () {
    $('#singletonClassAddDialogRoot').show();
}
ShowRegistrationDialog = function () {
    $('#registrationDialogRoot').show();
}
EditUserInfo = function () {
    editMode = 'edit';
    $('#registrationUserName').val(viewModel.user().userName());
    $('#registrationMajor').val(viewModel.user().major());
    $('#registrationUserName').prop('readonly', 'true');
    ShowRegistrationDialog();
}
SubmitRegistrationInformation = function () {
    var userName = $('#registrationUserName').val();
    var password = $('#registrationPassword1').val();
    var confirmPswd = $('#registrationPassword2').val();
    var major = $('#registrationMajor').val();
    if (editMode === 'new') {
        $.ajax({
            url: window.location.origin + '/Account/CheckUserName',
            data: JSON.stringify({ UserName: userName }),
            type: 'POST',
            contentType: 'application/json',
            success: function (data, textStatus, jqXHR) {
                if (!data.Success) {
                    Alert(data.Message);
                    return;
                }

                var errorMessage = '';
                if (!data.Payload) {
                    errorMessage += 'User name not available\n';
                }
                if (password != confirmPswd) {
                    errorMessage += 'Passowrds do not match\n';
                }
                if (major.length < 1) {
                    errorMessage += 'Please enter a major or "Undeclared" if you do not have one yet';
                }
                $('#registrationPassword1').val('');
                $('#registrationPassword2').val('');
                if (errorMessage !== '') {
                    Alert(errorMessage);
                    return;
                }

                var registrationRequest = { UserName: userName, Password: password, Major: major };
                $.ajax({
                    url: window.location.origin + '/Account/Register',
                    data: JSON.stringify(registrationRequest),
                    type: 'POST',
                    contentType: 'application/json',
                    success: function (data, textStatus, jqXHR) {
                        if (!data.Success) {
                            Alert(data.Message);
                            $('#blockingDiv').hide();
                            return;
                        }

                        var appUser = data.Payload;
                        viewModel.user(new User(appUser.SessionID, appUser.UserName, appUser.Major));
                        ko.utils.arrayForEach(appUser.Advisors, function (advisor) {
                            viewModel.user().advisors.push(new Advisor(advisor.Name, advisor.EMail));
                        });

                        document.cookie = 'CAPPamariCredentials=' + appUser.SessionID + '#' + appUser.UserName + ';';

                        $('#registrationDialogRoot').hide();

                        $('#blockingDiv').hide();
                        RedisplayHeader();
                    }
                });
                $('#blockingDivSpan').text('Registering...');
                $('#blockingDiv').show();
            },
            error: function () {
                Alert('There is an issue with the server, please try again later');
                $('#blockingDiv').hide();
            }
        });
        $('#blockingDivSpan').text('Checking UserName...');
        $('#blockingDiv').show();
    } else if (editMode === 'edit') {
        var registrationRequest = { UserName: userName, Password: password, Major: major };
        if (password != confirmPswd) {
            Alert('Passwords do not match!');
            $('#registrationPassword1').val('');
            $('#registrationPassword2').val('');
            return;
        }
        $('#registrationPassword1').val('');
        $('#registrationPassword2').val('');
        $.ajax({
            url: window.location.origin + '/api/User/UpdateUser',
            data: JSON.stringify(registrationRequest),
            type: 'POST',
            contentType: 'application/json',
            success: function (data, textStatus, jqXHR) {
                if (!data.Success) {
                    Alert(data.Message);
                    $('#blockingDiv').hide();
                    return;
                }

                var appUser = data.Payload;
                viewModel.user(new User(appUser.SessionID, appUser.UserName, appUser.Major));
                ko.utils.arrayForEach(appUser.Advisors, function (advisor) {
                    viewModel.user().advisors.push(new Advisor(advisor.Name, advisor.EMail));
                });

                document.cookie = 'CAPPamariCredentials=' + appUser.SessionID + '#' + appUser.UserName + ';';

                $('#registrationDialogRoot').hide();

                $('#blockingDiv').hide();
                RedisplayHeader();
            }
        });
        $('#blockingDivSpan').text('Updating...');
        $('#blockingDiv').show();
    }
}
CancelRegistration = function () {
    $('#registrationUserName').val('');
    $('#registrationPassword1').val('');
    $('#registrationPassword2').val('');
    $('#registrationMajor').val('');

    $('#registrationDialogRoot').hide();
}
SignInButtonClick = function () {
    var userName = $('#loginHeaderUserName').val();
    var password = $('#loginHeaderPassword').val();
    SignInUser(userName, password);
    $('#loginHeaderPassword').val('');
}
ResizeDisplay = function () {
    var headerBarRoot = $('#headerBarRoot');
    var mainScreen = $('#mainScreen');
    var mainBody = $('#mainBody');
    var sideBarRoot = $('#sidebarRoot');
    var openCloseSidebarDiv = $('#openCloseSidebarDiv');
    var sideBarRootWidth = sideBarRoot.outerWidth();

    mainScreen.outerHeight($(window).innerHeight() - headerBarRoot.outerHeight());
    mainBody.css('margin-left', sideBarRootWidth);
    mainBody.outerWidth(mainScreen.innerWidth() - sideBarRootWidth);
    openCloseSidebarDiv.css('padding-top', mainBody.innerHeight() / 2);
    openCloseSidebarDiv.height(mainBody.innerHeight() / 2);
    $('#courses').height(mainScreen.innerHeight() - $('#addClassButton').outerHeight() - $('#autopopButton').outerHeight());
    alertDialog.dialog({
        width: mainScreen.innerWidth() - 20
    });
}
ToggleSidebar = function () {
    var sidebarWrapper = $('#sidebarWrapper');
    var arrowSpan = $('#arrowSpan');

    if (sidebarWrapper.css('display') === 'none') {
        sidebarWrapper.show();
        arrowSpan.text('<');
    } else {
        sidebarWrapper.hide();
        arrowSpan.text('>');
    }
    ResizeDisplay();
}
RedisplayHeader = function () {
    var userHeader = $('#userHeader');
    var loginHeader = $('#loginHeader');
    if (viewModel.user() === null) {
        userHeader.hide();
        loginHeader.show();
    } else {
        loginHeader.hide();
        userHeader.show();
    }
    ResizeDisplay();
}
LoadUserFromCookie = function () {
    var cookies = document.cookie.split(';');
    var userCookie = '';
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        while (cookie.charAt(0) === ' ') cookie = cookie.substring(1);
        if (cookie.indexOf('CAPPamariCredentials=') !== -1) userCookie = cookie.substring(21, cookie.length);
    }
    if (userCookie === '') return false;
    $.ajax({
        url: window.location.origin + '/api/User/LoadFromUserSessionCookie',
        data: JSON.stringify(userCookie),
        type: 'POST',
        contentType: 'application/json',
        success: function (data, textStatus, jqXHR) {
            if (data.Success) {
                var appUser = data.Payload;
                viewModel.user(new User(appUser.SessionID, appUser.UserName, appUser.Major));
                ko.utils.arrayForEach(appUser.Advisors, function (advisor) {
                    viewModel.user().advisors.push(new Advisor(advisor.Name, advisor.EMail));
                });
                RedisplayHeader();
                viewModel.loadCAPPReport();
            } else {
                $('#blockingDiv').hide();
            }
        },
        error: function () {
            $('#blockingDiv').hide();
        }
    });
    $('#blockingDivSpan').text('Signing you in...');
    $('#blockingDiv').show();
}
SignInUser = function (userName, password) {
    var jsonData = { UserName: userName, Password: password };
    $.ajax({
        url: window.location.origin + '/Account/Login',
        data: JSON.stringify(jsonData),
        type: 'POST',
        contentType: 'application/json',
        success: function (data, textStatus, jqXHR) {
            if (!data.Success) {
                Alert(data.Message);
                $('#blockingDiv').hide();
                return;
            }

            var appUser = data.Payload;
            viewModel.user(new User(appUser.SessionID, appUser.UserName, appUser.Major));
            ko.utils.arrayForEach(appUser.Advisors, function (advisor) {
                viewModel.user().advisors.push(new Advisor(advisor.Name, advisor.EMail));
            });

            document.cookie = 'CAPPamariCredentials=' + appUser.SessionID + '#' + appUser.UserName + ';';
            RedisplayHeader();
            viewModel.loadCAPPReport();
        },
        error: function () {
            Alert('There is an issue with the server, please try again later');
            $('#blockingDiv').hide();
        }
    });
    $('#blockingDivSpan').text('Signing you in...');
    $('#blockingDiv').show();
}
ToJSONCourse = function (course) {
    return {
        department: course.department(),
        number: course.number(),
        semester: course.semester(),
        passNoCredit: course.passNoCredit,
        grade: course.grade(),
        credits: course.credits(),
        commIntensive: course.commIntensive
    };
}

User = function (sessionID, userName, major) {
    /* Properties */
    var self = this;
    self.userName = ko.observable(userName);
    self.major = ko.observable(major);
    self.sessionID = sessionID;
    self.advisors = ko.observableArray([]);

    /* Functions */
    self.signOut = function () {
        var jsonData = { UserName: self.userName() };
        $.ajax({
            url: window.location.origin + '/Account/Logout',
            data: JSON.stringify(jsonData),
            type: 'POST',
            contentType: 'application/json',
            success: function (data, textStatus, jqXHR) {
                $('#blockingDiv').hide();
            },
            error: function () {
                $('#blockingDiv').hide();
            }
        });
        $('#blockingDivSpan').text('Logging you out...');
        $('#blockingDiv').show();
        viewModel.user(null);
        viewModel.clearCAPPReport();
        RedisplayHeader();
        $('#openCloseSidebarDiv').hide();
    }
}
Advisor = function (name, emailAddress) {
    /* Properties */
    var self = this;
    self.name = ko.observable(name);
    self.emailAddress = ko.observable(emailAddress);
}
Course = function (department, number, semester, passNoCredit, grade, credits, commIntensive) {
    /* Properties */
    var self = this;
    self.department = ko.observable(department);
    self.number = ko.observable(number);
    self.semester = ko.observable(semester);
    self.passNoCredit = passNoCredit;
    self.grade = ko.observable(grade);
    self.credits = ko.observable(credits);
    self.commIntensive = commIntensive;
}
RequirementSet = function (name) {
    /* Properties */
    var self = this;
    self.name = ko.observable(name);
    self.appliedCourses = ko.observableArray([]);

    /* Functions */
    self.addCourse = function (course) {
        self.appliedCourses.push(course);
    }
    self.removeCourse = function (course) {
        self.appliedCourses.pop(course);
    }
}
ViewModel = function () {
    /* Properties */
    var self = this;
    self.unassignedCourses = ko.observableArray([]);
    self.requirementSets = ko.observableArray([]);
    self.user = ko.observable(null);

    /* Functions */
    self.addNewCouse = function (course) {
        self.unassignedCourses.push(course);
        MakeCoursesDraggable();
    }
    self.print = function () {
        var url = window.location.origin + '/Home/Print?UserName=' + self.user().userName();
        window.open(url, '_blank');
    }
    self.emailToAdvisor = function () {
        EmailToAdvisor(self.advisor());
    }
    self.editAdvisorInfo = function () {
        EditAdvisorInfo(self.advisor());
    }
    self.deleteAdvisor = function () {
        DeleteAdvisor(self.advisor());
    }
    self.clearCAPPReport = function () {
        self.unassignedCourses([]);
        self.requirementSets([]);
    }
    self.reloadCAPPReport = function () {
        self.clearCAPPReport();
        self.loadCAPPReport();
    }
    self.setCAPPReport = function (cappReport) {
        self.clearCAPPReport();
        self.requirementSets.push(new RequirementSet('CAPP Report Requirements'));
        ko.utils.arrayForEach(cappReport.RequirementSets, function (RequirementSetModel) {
            if (RequirementSetModel.Name === 'Unapplied Courses') {
                ko.utils.arrayForEach(RequirementSetModel.AppliedCourses, function (CourseModel) {
                    self.unassignedCourses.push(new Course(CourseModel.DepartmentCode, CourseModel.CourseNumber, CourseModel.Semester, CourseModel.PassNoCredit, CourseModel.Grade, CourseModel.Credits, CourseModel.CommIntensive));
                });
            } else {
                var newRequirementSet = new RequirementSet(RequirementSetModel.Name);
                ko.utils.arrayForEach(RequirementSetModel.AppliedCourses, function (CourseModel) {
                    newRequirementSet.addCourse(new Course(CourseModel.DepartmentCode, CourseModel.CourseNumber, CourseModel.Semester, CourseModel.PassNoCredit, CourseModel.Grade, CourseModel.Credits, CourseModel.CommIntensive));
                });
                self.requirementSets.push(newRequirementSet);
            }
        });
        SetupDragAndDrop();
    }
    self.loadCAPPReport = function () {
        $.ajax({
            url: window.location.origin + '/api/User/GetCAPPReport',
            data: JSON.stringify(self.user().userName()), 
            type: 'POST',
            contentType: 'application/json',
            success: function (data, textStatus, jqXHR) {
                if (!data.Success) {
                    Alert(data.Message);
                    $('#blockingDiv').hide();
                    return;
                }
                var cappReport = data.Payload;
                self.requirementSets.push(new RequirementSet('CAPP Report Requirements'));
                ko.utils.arrayForEach(cappReport.RequirementSets, function (RequirementSetModel) {
                    if (RequirementSetModel.Name === 'Unapplied Courses') {
                        ko.utils.arrayForEach(RequirementSetModel.AppliedCourses, function (CourseModel) {
                            self.unassignedCourses.push(new Course(CourseModel.DepartmentCode, CourseModel.CourseNumber, CourseModel.Semester, CourseModel.PassNoCredit, CourseModel.Grade, CourseModel.Credits, CourseModel.CommIntensive));
                        });
                    } else {
                        var newRequirementSet = new RequirementSet(RequirementSetModel.Name);
                        ko.utils.arrayForEach(RequirementSetModel.AppliedCourses, function (CourseModel) {
                            newRequirementSet.addCourse(new Course(CourseModel.DepartmentCode, CourseModel.CourseNumber, CourseModel.Semester, CourseModel.PassNoCredit, CourseModel.Grade, CourseModel.Credits, CourseModel.CommIntensive));
                        });
                        self.requirementSets.push(newRequirementSet);
                    }
                });
                $('#openCloseSidebarDiv').show();
                SetupDragAndDrop();
                ResizeDisplay();
                $('#blockingDiv').hide();
            },
            error: function () {
                Alert('There is an issue with the server, please try again later');
                $('#blockingDiv').hide();
            }
        });
        $('#blockingDivSpan').text('Loading your CAPP Report...');
        $('#blockingDiv').show();
    }
}