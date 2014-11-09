var user = null;
var viewModel = null;

$(window).resize(function () {
    ResizeDisplay();
});
$(window).load(function () {
    viewModel = new ViewModel();
    // try to load user from cookie

    // insertion of dummy data
    viewModel.addNewRequirementSet(new RequirementSet('Communication Intensive'));
    viewModel.addNewRequirementSet(new RequirementSet('Math'));
    viewModel.addNewRequirementSet(new RequirementSet('Science'));
    viewModel.addNewRequirementSet(new RequirementSet('Computer Science'));
    viewModel.addNewRequirementSet(new RequirementSet('Computer Science Option'));
    viewModel.addNewRequirementSet(new RequirementSet('HASS'));
    viewModel.addNewCouse(new Course("CSCI", 4000, "F14", true, null, 1));
    ko.applyBindings(viewModel);

    // drag'n drop code
    SetupDragAndDrop();

    ResizeDisplay();
});

MakeCoursesDraggable = function () {
    $(".course").draggable({
        appendTo: "body",
        helper: "clone",
        revert: true,
        containment: "#mainScreen",
    });
}

SetupDragAndDrop = function () {
    $(".requirementBox").accordion({
        collapsible: true,
    });
    $(".requirementBox").droppable({
        drop: function (event, ui) {
            ui.draggable.appendTo($(this).find(".courses"));
            ui.helper.remove();
            $(".requirementBox").accordion("resize");
            if ($(this).accordion("option", "active") === false)
            {
                $(this).accordion("option", "active", 0);
            }
        }
    });
    $("#sidebarWrapper #courses").droppable({
        drop: function (event, ui) {
            ui.draggable.appendTo($(this));
            ui.helper.remove();
            $(".requirementBox").accordion("resize");
        }
    });
    MakeCoursesDraggable();
}

SubmitSingletonClassAddInformation = function () {
    var deptCode = $('#singletonDepartment').val();
    var courseNumber = $('#singletonCourseNumber').val();
    var semesterCode = $('#singletonSemesterCode').val();
    var passNoCredit = $('#singletonPassNoCredit')[0].checked;
    var grade = $('#singletonGrade').val();
    var credits = $('#singletonCredits').val();

    if (deptCode.length !== 4) {
        alert('Please enter valid Department Code');
        return;
    }
    if (courseNumber.length !== 4) {
        alert('Please enter valid Course Number');
        return;
    }
    if (semesterCode.length !== 3) {
        alert('Please enter valid Semester Code');
        return;
    }
    if (credits.length !== 1) {
        alert('Please enter number of credits');
        return;
    }

    var course = new Course(deptCode, courseNumber, semesterCode, passNoCredit, grade, credits);
    viewModel.addNewCouse(course);

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
    // add validators if needed
}
ShowRegistrationDialog = function () {
    $('#registrationDialogRoot').show();
    // add validators if needed
}
SubmitRegistrationInformation = function () {
    var userName = $('#registrationUserName').val();
    var password = $('#registrationPassword1').val();
    var confirmPswd = $('#registrationPassword2').val();
    var major = $('#registrationMajor').val();

    $.ajax({
        url: window.location.origin + '/Account/CheckUserName',
        data: JSON.stringify({ UserName: userName }),
        type: 'POST',
        contentType: 'application/json',
        success: function (data, textStatus, jqXHR) {
            if (!data.Success) {
                alert(data.Message);
                return;
            }

            if (!data.Payload) {
                alert('User name ' + userName + ' not available');
                return;
            }
            if (password != confirmPswd) {
                alert('Passwords do not match');
                $('#registrationPassword1').val('');
                $('#registrationPassword2').val('');
                return;
            }
            if (major.length < 1) {
                alert('Please enter a major or "Undeclared" if you do not have one yet');
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
                        alert(data.Message);
                        return;
                    }

                    var appUser = data.Payload;
                    viewModel.user(new User(appUser.SessionID, appUser.UserName, appUser.Major));
                    ko.utils.arrayForEach(appUser.Advisors, function (advisor) {
                        user.advisors.push(new Advisor(advisor.Name, advisor.EMail));
                    });

                    // set cookie

                    $('#registrationDialogRoot').hide();

                    RedisplayHeader();
                }
            });
        },
        error: function () {
            alert('There is an issue with the server, please try again later');
            waiting = false;
        }
    });
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

    $('#headerBarWrapper').outerWidth(headerBarRoot.innerWidth());
    mainScreen.outerHeight($(window).innerHeight() - headerBarRoot.outerHeight());
    mainBody.css('margin-left', sideBarRootWidth);
    mainBody.outerWidth(mainScreen.innerWidth() - sideBarRootWidth);
    openCloseSidebarDiv.css('padding-top', mainBody.innerHeight() / 2);
    openCloseSidebarDiv.height(mainBody.innerHeight() / 2);
    $('#courses').height(mainScreen.innerHeight() - $('#addClassButton').outerHeight());
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
SignInUser = function (userName, password) {
    var jsonData = { UserName: userName, Password: password };
    $.ajax({
        url: window.location.origin + '/Account/Login',
        data: JSON.stringify(jsonData),
        type: 'POST',
        contentType: 'application/json',
        success: function (data, textStatus, jqXHR) {
            if (!data.Success) {
                alert(data.Message);
                return;
            }

            var appUser = data.Payload;
            user = new User(appUser.SessionID, appUser.UserName, appUser.Major);
            ko.utils.arrayForEach(appUser.Advisors, function (advisor) {
                user.advisors.push(new Advisor(advisor.Name, advisor.EMail));
            });

            // set cookie

            RedisplayHeader();
        },
        error: function () {
            alert('There is an issue with the server, please try again later');
        }
    });
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
        var jsonData = { UserName: self.userName };
        $.ajax({
            url: window.location.origin + '/Account/Logout',
            data: JSON.stringify(jsonData),
            type: 'POST',
            contentType: 'application/json'
        });
        user = null;

        RedisplayHeader();
    }
}
Advisor = function (name, emailAddress) {
    /* Properties */
    var self = this;
    self.name = ko.observable(name);
    self.emailAddress = ko.observable(emailAddress);
}
Course = function (department, number, semester, passNoCredit, grade, credits) {
    /* Properties */
    var self = this;
    self.department = ko.observable(department);
    self.number = ko.observable(number);
    self.semester = ko.observable(semester);
    self.passNoCredit = passNoCredit;
    self.grade = ko.observable(grade);
    self.credits = ko.observable(credits);
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
        // add logic to detect where it should go and prompt user if correct
        self.unassignedCourses.push(course);
        MakeCoursesDraggable();
    }
    self.addNewRequirementSet = function (requirementSet) {
        self.requirementSets.push(requirementSet);
    }
}