var editMode = 'new';
var viewModel = null;
var draggedCourse = null;
var requirementBox = null;
var alertTimer = null;
var sourceRequirementSet = null;

$(window).resize(function() {
    ResizeDisplay();
});
$(window).load(function() {
    viewModel = new ViewModel();

    LoadUserFromCookie();

    if (!window.File || !window.FileReader) {
        HideCsvImportAbility();
    }

    ko.applyBindings(viewModel);

    $(document).tooltip();
    RedisplayHeader();
});

Alert = function(success, text) {
    var alertClass;
    var alert = $('#alert');
    if (success) {
        alertClass = 'ui-state-highlight';
    } else {
        alertClass = 'ui-state-error';
    }
    if (alertTimer !== null) {
        window.clearTimeout(alertTimer);
    }
    alert.stop(true).removeClass('ui-state-highlight ui-state-error').addClass(alertClass).text(text).fadeIn('fast');
    alertTimer = setTimeout(function() {
        alert.fadeOut('fast').removeClass(alertClass);
    }, 5000);
};
AutoPopulateUnappliedCourses = function() {
    $.ajax({
        url: window.location.origin + '/api/Course/AutoPopulateUnappliedCourses',
        data: JSON.stringify(viewModel.user().username()),
        type: 'POST',
        contentType: 'application/json',
        success: function(data) {
            if (!data.Success) {
                $('#blockingDiv').hide();
                Alert(false, data.Message);
                return;
            }

            viewModel.SetCappReport(data.Payload);
            $('#blockingDiv').hide();
            Alert(true, data.Message);
        },
        error: function() {
            $('#blockingDiv').hide();
            Alert(false, 'There is a problem with the server.  Please try again later');
        }
    });
    $('#blockingDivSpan').text('Auto-populating courses, this may take a bit...');
    $('#blockingDiv').show();
};
RemoveCourse = function(course) {
    var removeCourseRequest = {
        Username: viewModel.user().username(),
        CourseToRemove: {
            DepartmentCode: course.department(),
            CourseNumber: course.number(),
            Grade: course.grade(),
            Credits: course.credits(),
            Semester: course.semester(),
            PassNoCredit: course.passNoCredit,
            CommIntensive: course.commIntensive,
            RequirementSetName: course.requirementSetName()
        }
    };
    $.ajax({
        url: window.location.origin + '/api/Course/RemoveCourse',
        data: JSON.stringify(removeCourseRequest),
        type: 'POST',
        contentType: 'application/json',
        success: function(data) {
            if (!data.Success || !data.Payload) {
                Alert(false, data.Message);
                return;
            }

            var requirementSet = viewModel.GetRequirementSet(course);
            requirementSet.RemoveCourse(course);
            if (requirementSet.name() !== 'Unapplied Courses') {
                UpdateFulfilledStatus(requirementSet);
            }
            $('.accordion').accordion('refresh');
            Alert(true, data.Message);
        },
        error: function() {
            Alert(false, 'There is an issue with the server, please try again later');
        }
    });
    Alert(true, 'Removing course...');
};
HideCsvImportAbility = function() {
    var importTable = $('#addCourseDialogRoot').find('table');
    importTable.find('tr:nth-child(9n)').hide();
    importTable.find('tr:nth-child(10n)').hide();
    importTable.find('tr:nth-child(11n)').hide();
    importTable.find('tr:nth-child(12n)').hide();
};
EmailToAdvisor = function(advisor) {
    var emailRequest = {
        Username: viewModel.user().username(),
        Advisor: {
            Name: advisor.name(),
            Email: advisor.emailAddress()
        }
    };
    $.ajax({
        url: window.location.origin + '/api/User/EmailToAdvisor',
        data: JSON.stringify(emailRequest),
        type: 'POST',
        contentType: 'application/json',
        success: function(data) {
            Alert(true, data.Message);
        },
        error: function() {
            Alert(false, 'There is an issue with the server, please try again later');
        }
    });
    Alert(true, 'Sending email...');
};
RemoveAdvisor = function(advisor) {
    var removeAdvisorRequest = {
        Username: viewModel.user().username(),
        Advisor: {
            Name: advisor.name(),
            Email: advisor.emailAddress()
        }
    };
    $.ajax({
        url: window.location.origin + '/api/User/RemoveAdvisor',
        data: JSON.stringify(removeAdvisorRequest),
        type: 'POST',
        contentType: 'application/json',
        success: function(data) {
            if (!data.Success || !data.Payload) {
                $('#blockingDiv').hide();
                Alert(false, data.Message);
                return;
            }

            viewModel.user().advisors.remove(advisor);
            RedisplayHeader();
            Alert(true, data.Message);
        },
        error: function() {
            Alert(false, 'There is an issue with the server, please try again later');
        }
    });
    Alert(true, 'Deleting advisor...');
};
EditAdvisorInfo = function(advisor) {
    editMode = 'edit';
    $('#advisorName').val(advisor.name());
    $('#advisorEmail').val(advisor.emailAddress());
    $('#advisorName').attr('disabled', true);
    ShowAdvisorDialog();
};
ShowAdvisorDialog = function() {
    $('#advisorDialogRoot').show();
};
AddNewAdvisor = function() {
    editMode = 'new';
    $('#advisorName').val('');
    $('#advisorEmail').val('');
    $('#advisorName').attr('disabled', false);
    ShowAdvisorDialog();
};
SubmitAdvisorInformation = function() {
    var name = $('#advisorName').val();
    var email = $('#advisorEmail').val();
    if (editMode === 'new') {
        var newAdvisorRequest = { Username: viewModel.user().username(), Advisor: { Name: name, Email: email } };
        $.ajax({
            url: window.location.origin + '/api/User/AddAdvisor',
            data: JSON.stringify(newAdvisorRequest),
            type: 'POST',
            contentType: 'application/json',
            success: function(data) {
                if (!data.Success || !data.Payload) {
                    Alert(false, data.Message);
                    return;
                }

                viewModel.user().advisors.push(new Advisor(name, email));
                RedisplayHeader();
                $('#blockingDiv').hide();
                Alert(true, data.Message);
            },
            error: function() {
                Alert(false, 'There is something wrong with the server, please try again later');
            }
        });
        Alert(true, 'Adding advisor...');
    } else if (editMode === 'edit') {
        var editAdvisorRequest = { Name: name, Email: email };
        $.ajax({
            url: window.location.origin + '/api/User/UpdateAdvisor',
            data: JSON.stringify(editAdvisorRequest),
            type: 'POST',
            contentType: 'application/json',
            success: function(data) {
                if (!data.Success || !data.Payload) {
                    Alert(false, data.Message);
                    return;
                }

                ko.utils.arrayForEach(viewModel.user().advisors(), function(advisor) {
                    if (advisor.name() === name) {
                        advisor.emailAddress(email);
                    }
                });

                RedisplayHeader();
                Alert(true, data.Message);
            },
            error: function() {
                Alert(false, 'There is something wrong with the server, please try again later');
            }
        });
        Alert(true, 'Updating advisor...');
    }
    $('#advisorDialogRoot').hide();
};
CancelAdvisorDialog = function() {
    $('#advisorDialogRoot').hide();
};
UpdateFulfilledStatus = function(requirementBox) {
    var isFulfilledRequest = { Username: viewModel.user().username(), RequirementSetName: requirementBox.name() };
    $.ajax({
        url: window.location.origin + '/api/Course/CheckFulfillment',
        data: JSON.stringify(isFulfilledRequest),
        type: 'POST',
        contentType: 'application/json',
        success: function(data) {
            requirementBox.isFull(data.Payload);
        },
        error: function() {
            Alert(false, 'There is an error with the server.  Please try again later');
        }
    });
};
MakeCoursesDraggable = function() {
    $('.course').draggable({
        appendTo: 'body',
        helper: 'clone',
        revert: 'invalid',
        containment: '#mainScreen',
        start: function() {
            sourceRequirementSet = ko.dataFor($(this).parent()[0]);
        }
    });
};
SetupDragAndDrop = function() {
    $('.accordion').accordion({
        collapsible: true,
    });
    $('.requirementBox').droppable({
        drop: function(event, ui) {
            var requirementBox = $(this);
            var course = ko.dataFor(ui.draggable[0]);
            var requirementSet = ko.dataFor(this);
            var moveCourseRequest = {
                Username: viewModel.user().username(),
                CourseToMove: {
                    DepartmentCode: course.department(),
                    CourseNumber: course.number(),
                    Grade: course.grade(),
                    Credits: course.credits(),
                    Semester: course.semester(),
                    PassNoCredit: course.passNoCredit,
                    CommIntensive: course.commIntensive,
                    RequirementSetName: requirementSet.name()
                },
            };
            $.ajax({
                url: window.location.origin + '/api/Course/MoveCourse',
                data: JSON.stringify(moveCourseRequest),
                type: 'POST',
                contentType: 'application/json',
                success: function(data) {
                    if (!data.Success || !data.Payload) {
                        Alert(false, data.Message);
                        return;
                    }

                    var temp = sourceRequirementSet.RemoveCourse(course);
                    temp.requirementSetName(requirementSet.name());
                    requirementSet.AddCourse(temp);
                    UpdateFulfilledStatus(requirementSet);
                    if (sourceRequirementSet.name() !== 'Unapplied Courses') {
                        UpdateFulfilledStatus(sourceRequirementSet);
                    }
                    $('.accordion').accordion('refresh');
                    MakeCoursesDraggable();
                    Alert(true, data.Message);
                },
                error: function() {
                    Alert(false, 'There is a problem with the server, please try again later');
                }
            });
            Alert(true, 'Moving course...');
        }
    });
    MakeCoursesDraggable();
};
ImportCsvFile = function() {
    if (!window.File || !window.FileReader) {
        Alert(false, 'Your browser does not support file importing');
        return;
    }

    var autoPopulation = $('#csvAutoPopulateCheckbox')[0].checked;
    var fileToRead = $('#csvFileInput')[0].files[0];
    var reader = new FileReader();
    reader.readAsText(fileToRead);
    reader.onload = function(event) {
        var csvData = event.target.result;
        var csvImportRequest = { Username: viewModel.user().username(), CsvData: csvData, AutoPopulate: autoPopulation };
        $.ajax({
            url: window.location.origin + '/api/Course/AddCsvFile',
            data: JSON.stringify(csvImportRequest),
            type: 'POST',
            contentType: 'application/json',
            success: function(data) {
                viewModel.SetCappReport(data.Payload);
                $('#addCourseDialogRoot').hide();
                $('#blockingDiv').hide();
                Alert(true, data.Message);
            },
            error: function() {
                $('#blockingDiv').hide();
                Alert(false, 'There is an error with the server.  Please try again later');
            }
        });
        $('#blockingDivSpan').text('Processing courses, this may take a bit...');
        $('#blockingDiv').show();
    };
    reader.onerror = function() {
        $('#addCourseDialogRoot').hide();
        $('#blockingDiv').hide();
        Alert(false, 'Unable to read file');
    };
    $('#blockingDivSpan').text('Reading in CSV...');
    $('#blockingDiv').show();
};
SubmitAddClassInformation = function() {
    var deptCode = $('#addCourseDepartment').val();
    var courseNumber = $('#addCourseCourseNumber').val();
    var semesterCode = $('#addCourseSemesterCode').val();
    var passNoCredit = $('#addCoursePassNoCredit')[0].checked;
    var commIntensive = $('#addCourseCommIntensive')[0].checked;
    var grade = $('#addCourseGrade').val();
    var credits = $('#addCourseCredits').val();

    var errorMessage = '';
    // make sure deptCode is a 4 letter all caps code
    if (!deptCode.match(/^[A-Z]{4}$/)) {
        errorMessage += 'Please enter a valid Department Code\n';
    }
    if (!courseNumber.match(/^[1|2|4|6][x|0-9]{3}$/)) {
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
        Alert(false, errorMessage);
        return;
    }

    var newCourseRequest = {
        Username: viewModel.user().username(),
        NewCourse: {
            DepartmentCode: deptCode,
            CourseNumber: courseNumber,
            Grade: grade,
            Credits: credits,
            Semester: semesterCode,
            PassNoCredit: passNoCredit,
            CommIntensive: commIntensive,
            RequirementSetName: 'Unapplied Courses'
        }
    };
    $.ajax({
        url: window.location.origin + '/api/Course/AddNewCourse',
        data: JSON.stringify(newCourseRequest),
        type: 'POST',
        contentType: 'application/json',
        success: function(data) {
            if (!data.Success) {
                Alert(false, data.Message);
                return;
            }

            var course = new Course(deptCode, courseNumber, semesterCode, passNoCredit, grade, credits, commIntensive, 'Unapplied Courses');
            viewModel.AddCourse(course);
            $('#addCourseDepartment').val('');
            $('#addCourseCourseNumber').val('');
            $('#addCourseSemesterCode').val('');
            $('#addCoursePassNoCredit')[0].checked = false;
            $('#addCourseCommIntensive')[0].checked = false;
            $('#addCourseGrade').val('');
            $('#addCourseCredits').val('');
            var fileDialog = $('#csvFileInput');
            fileDialog.replaceWith(fileDialog.val('').clone(true));
            $('#blockingDiv').hide();
            Alert(true, data.Message);
        },
        error: function() {
            Alert(false, 'There is an issue with the server, please try again later');
        }
    });
    $('#addCourseDialogRoot').hide();
    Alert(true, 'Adding your course...');
};
CancelAddClass = function() {
    $('#addCourseDepartment').val('');
    $('#addCourseCourseNumber').val('');
    $('#addCourseSemesterCode').val('');
    $('#addCoursePassNoCredit')[0].checked = false;
    $('#addCourseCommIntensive')[0].checked = false;
    $('#addCourseGrade').val('');
    $('#addCourseCredits').val('');

    $('#addCourseDialogRoot').hide();
};
ShowAddClassDialog = function() {
    $(".addCourseForm").show();
    $('#addCourseDialogRoot').show();
};
ShowRegistrationDialog = function() {
    $('#registrationDialogRoot').show();
};
EditUserInfo = function() {
    editMode = 'edit';
    $('#registrationUsername').val(viewModel.user().username());
    $('#registrationMajor').val(viewModel.user().major());
    $('#registrationUsername').attr('disabled', true);
    ShowRegistrationDialog();
};
SubmitRegistrationInformation = function() {
    var username = $('#registrationUsername').val();
    var password = $('#registrationPassword1').val();
    var confirmPswd = $('#registrationPassword2').val();
    var major = $('#registrationMajor').val();
    if (editMode === 'new') {
        $.ajax({
            url: window.location.origin + '/Account/CheckUsername',
            data: JSON.stringify({ Username: username }),
            type: 'POST',
            contentType: 'application/json',
            success: function(data) {
                if (!data.Success) {
                    $('#blockingDiv').hide();
                    Alert(false, data.Message);
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
                    $('#blockingDiv').hide();
                    Alert(false, errorMessage);
                    return;
                }

                var registrationRequest = { Username: username, Password: password, Major: major };
                $.ajax({
                    url: window.location.origin + '/Account/Register',
                    data: JSON.stringify(registrationRequest),
                    type: 'POST',
                    contentType: 'application/json',
                    success: function(data) {
                        if (!data.Success) {
                            $('#blockingDiv').hide();
                            Alert(false, data.Message);
                            return;
                        }

                        var appUser = data.Payload;
                        viewModel.user(new User(appUser.SessionId, appUser.Username, appUser.Major));
                        ko.utils.arrayForEach(appUser.Advisors, function(advisor) {
                            viewModel.user().advisors.push(new Advisor(advisor.Name, advisor.Email));
                        });

                        document.cookie = 'CAPPamariCredentials=' + appUser.SessionId + '#' + appUser.Username + ';';

                        $('#registrationDialogRoot').hide();
                        $('#sidebarRoot').show();
                        SetupDragAndDrop();
                        RedisplayHeader();
                        $('#blockingDiv').hide();
                        viewModel.LoadCappReport();
                    },
                    error: function() {
                        $('#blockingDiv').hide();
                        Alert(false, 'There is an issue with the server, please try again later');
                    }
                });
                $('#blockingDivSpan').text('Setting up your CAPP report, this may take a bit...');
                $('#blockingDiv').show();
            },
            error: function() {
                $('#blockingDiv').hide();
                Alert(false, 'There is an issue with the server, please try again later');
            }
        });
        $('#blockingDivSpan').text('Checking Username...');
        $('#blockingDiv').show();
    } else if (editMode === 'edit') {
        var registrationRequest = { Username: username, Password: password, Major: major };
        if (password != confirmPswd) {
            Alert(false, 'Passwords do not match!');
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
            success: function(data) {
                if (!data.Success) {
                    Alert(false, data.Message);
                    return;
                }

                var appUser = data.Payload;
                viewModel.user(new User(appUser.SessionId, appUser.Username, appUser.Major));
                ko.utils.arrayForEach(appUser.Advisors, function(advisor) {
                    viewModel.user().advisors.push(new Advisor(advisor.Name, advisor.Email));
                });

                document.cookie = 'CAPPamariCredentials=' + appUser.SessionId + '#' + appUser.Username + ';';

                $('#registrationDialogRoot').hide();
                RedisplayHeader();
                Alert(true, data.Message);
            },
            error: function() {
                Alert(false, 'There is an issue with the server, please try again later');
            }
        });
        Alert(true, 'Updating account...');
    }
};
CancelRegistration = function() {
    $('#registrationUsername').val('');
    $('#registrationPassword1').val('');
    $('#registrationPassword2').val('');
    $('#registrationMajor').val('');

    $('#registrationDialogRoot').hide();
};
SignInButtonClick = function() {
    var username = $('#loginHeaderUsername').val();
    var password = $('#loginHeaderPassword').val();
    SignInUser(username, password);
    $('#loginHeaderPassword').val('');
};
ResizeDisplay = function() {
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
    $('#unappliedCourses').height(mainScreen.innerHeight() - $('#addCourseButton').outerHeight() - $('#autoPopulationButton').outerHeight());
};
ToggleSidebar = function() {
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
};
RedisplayHeader = function() {
    var userHeader = $('#userHeader');
    var loginHeader = $('#loginHeader');
    var fontSize = parseInt(loginHeader.css('font-size'), 10);
    if (viewModel.user() === null) {
        userHeader.hide();
        loginHeader.css('min-height', fontSize * 4);
        loginHeader.show();
    } else {
        loginHeader.hide();
        userHeader.css('min-height', fontSize * 4 + (viewModel.user().advisors().length === 0 ? 1 : viewModel.user().advisors().length) * fontSize);
        userHeader.show();
    }
    ResizeDisplay();
};
LoadUserFromCookie = function() {
    var cookies = document.cookie.split(';');
    var userCookie = '';
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        while (cookie.charAt(0) === ' ') cookie = cookie.substring(1);
        if (cookie.indexOf('CAPPamariCredentials=') !== -1) userCookie = cookie.substring(21, cookie.length);
    }
    if (userCookie === '') return;
    $.ajax({
        url: window.location.origin + '/api/User/LoadFromUserSessionCookie',
        data: JSON.stringify(userCookie),
        type: 'POST',
        contentType: 'application/json',
        success: function(data) {
            if (!data.Success) {
                $('#blockingDiv').hide();
                return;
            }

            var appUser = data.Payload;
            viewModel.user(new User(appUser.SessionId, appUser.Username, appUser.Major));
            ko.utils.arrayForEach(appUser.Advisors, function(advisor) {
                viewModel.user().advisors.push(new Advisor(advisor.Name, advisor.Email));
            });
            RedisplayHeader();
            viewModel.LoadCappReport();
        },
        error: function() {
            $('#blockingDiv').hide();
            Alert(false, 'There is an issue with the server, please try again later');
        }
    });
    $('#blockingDivSpan').text('Signing you in...');
    $('#blockingDiv').show();
};
SignInUser = function(username, password) {
    var loginRequest = { Username: username, Password: password };
    $.ajax({
        url: window.location.origin + '/Account/Login',
        data: JSON.stringify(loginRequest),
        type: 'POST',
        contentType: 'application/json',
        success: function(data) {
            if (!data.Success) {
                $('#blockingDiv').hide();
                Alert(false, data.Message);
                return;
            }

            var appUser = data.Payload;
            viewModel.user(new User(appUser.SessionId, appUser.Username, appUser.Major));
            ko.utils.arrayForEach(appUser.Advisors, function(advisor) {
                viewModel.user().advisors.push(new Advisor(advisor.Name, advisor.Email));
            });

            document.cookie = 'CAPPamariCredentials=' + appUser.SessionId + '#' + appUser.Username + ';';
            RedisplayHeader();
            viewModel.LoadCappReport();
        },
        error: function() {
            $('#blockingDiv').hide();
            Alert(false, 'There is an issue with the server, please try again later');
        }
    });
    $('#blockingDivSpan').text('Signing you in...');
    $('#blockingDiv').show();
};
CheckForPncCheck = function() {
    if ($('#addCoursePassNoCredit')[0].checked) {
        $('#addCourseGradeRow').hide();
    } else {
        $('#addCourseGradeRow').show();
    }
};
CheckForCsvFile = function() {
    if ($('#csvFileInput')[0].files.length === 0) {
        $('.addCourseForm').show();
    } else {
        $('.addCourseForm').hide();
    }
};

User = function(sessionId, username, major) {
    /* Properties */
    var self = this;
    self.username = ko.observable(username);
    self.major = ko.observable(major);
    self.sessionId = sessionId;
    self.advisors = ko.observableArray([]);

    /* Functions */
    self.SignOut = function() {
        var jsonData = { Username: self.username() };
        $.ajax({
            url: window.location.origin + '/Account/Logout',
            data: JSON.stringify(jsonData),
            type: 'POST',
            contentType: 'application/json',
            success: function() {
                $('#blockingDiv').hide();
                Alert(true, 'Sucessfully logged out');
            },
            error: function() {
                $('#blockingDiv').hide();
                Alert(false, 'There is an issue with the server, please try again later');
            }
        });
        $('#blockingDivSpan').text('Logging you out...');
        $('#blockingDiv').show();
        viewModel.user(null);
        viewModel.ClearCappReport();
        RedisplayHeader();
        $('#sidebarRoot').hide();
    };
};
Advisor = function(name, emailAddress) {
    /* Properties */
    var self = this;
    self.name = ko.observable(name);
    self.emailAddress = ko.observable(emailAddress);
};
Course = function(department, number, semester, passNoCredit, grade, credits, commIntensive, requirementSetName) {
    /* Properties */
    var self = this;
    self.department = ko.observable(department);
    self.number = ko.observable(number);
    self.semester = ko.observable(semester);
    self.passNoCredit = passNoCredit;
    self.grade = ko.observable(grade);
    self.credits = ko.observable(credits);
    self.commIntensive = commIntensive;
    self.requirementSetName = ko.observable(requirementSetName);
};
RequirementSet = function(name, full) {
    /* Properties */
    var self = this;
    self.name = ko.observable(name);
    self.appliedCourses = ko.observableArray([]);
    self.isFull = ko.observable(full);

    /* Functions */
    self.AddCourse = function(course) {
        self.appliedCourses.push(course);
    };
    self.RemoveCourse = function(course) {
        return self.appliedCourses.remove(course)[0];
    };
};
ViewModel = function() {
    /* Properties */
    var self = this;
    self.requirementSets = ko.observableArray([]);
    self.user = ko.observable(null);

    /* Functions */
    self.GetRequirementSet = function(course) {
        for (var i = 0, j = self.requirementSets().length; i < j; i++) {
            if (self.requirementSets()[i].name() === course.requirementSetName()) {
                return self.requirementSets()[i];
            }
        }
        return null;
    };
    self.Print = function() {
        var url = window.location.origin + '/Home/Print?Username=' + self.user().username();
        window.open(url, '_blank');
    };
    self.ClearCappReport = function() {
        self.requirementSets([]);
    };
    self.ReloadCappReport = function() {
        self.ClearCappReport();
        self.LoadCappReport();
    };
    self.SetCappReport = function(cappReport) {
        self.ClearCappReport();
        self.requirementSets.push(new RequirementSet('CAPP Report Requirements'));
        ko.utils.arrayForEach(cappReport.RequirementSets, function(requirementSetModel) {
            var newRequirementSet = new RequirementSet(requirementSetModel.Name, requirementSetModel.IsFull);
            ko.utils.arrayForEach(requirementSetModel.AppliedCourses, function(courseModel) {
                newRequirementSet.AddCourse(new Course(courseModel.DepartmentCode, courseModel.CourseNumber, courseModel.Semester, courseModel.PassNoCredit, courseModel.Grade, courseModel.Credits, courseModel.CommIntensive, requirementSetModel.Name));
            });
            self.requirementSets.push(newRequirementSet);
        });
        SetupDragAndDrop();
        ResizeDisplay();
    };
    self.LoadCappReport = function() {
        $.ajax({
            url: window.location.origin + '/api/User/GetCappReport',
            data: JSON.stringify(self.user().username()),
            type: 'POST',
            contentType: 'application/json',
            success: function(data) {
                if (!data.Success) {
                    $('#blockingDiv').hide();
                    Alert(false, data.Message);
                    return;
                }
                var cappReport = data.Payload;
                ko.utils.arrayForEach(cappReport.RequirementSets, function(requirementSetModel) {
                    var newRequirementSet = new RequirementSet(requirementSetModel.Name, requirementSetModel.IsFull);
                    ko.utils.arrayForEach(requirementSetModel.AppliedCourses, function(courseModel) {
                        newRequirementSet.AddCourse(new Course(courseModel.DepartmentCode, courseModel.CourseNumber, courseModel.Semester, courseModel.PassNoCredit, courseModel.Grade, courseModel.Credits, courseModel.CommIntensive, requirementSetModel.Name));
                    });
                    self.requirementSets.push(newRequirementSet);
                });
                $('#sidebarRoot').show();
                SetupDragAndDrop();
                ResizeDisplay();
                $('#blockingDiv').hide();
                Alert(true, data.Message);
            },
            error: function() {
                $('#blockingDiv').hide();
                Alert(false, 'There is an issue with the server, please try again later');
            }
        });
        $('#blockingDivSpan').text('Loading your CAPP Report...');
        $('#blockingDiv').show();
    };
};