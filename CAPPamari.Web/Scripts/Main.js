var user = null;
var viewModel = null;

$(window).resize(function () {
    ResizeDisplay();
});
$(window).load(function () {
    viewModel = new ViewModel();
    // try to load user from cookie

    // UNCOMMENT WHEN WE HAVE UI ELEMENTS ACTUALLY SHOWING STUFF
    // ko.applyBindings(viewModel);

    ResizeDisplay();
});

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
                    user = new User(appUser.SessionID, appUser.UserName, appUser.Major);
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
    if (user === null) {
        userHeader.hide();
        loginHeader.show();
    } else {
        loginHeader.hide();
        userHeader.show();
        $('#userHeaderStudentName').text(user.userName);
        $('#userHeaderMajor').text(user.major);
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
    self.userName = userName;
    self.major = major;
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
    self.name = name;
    self.emailAddress = emailAddress;
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
ViewModel = function () {
    /* Properties */
    var self = this;
    self.unassignedCourses = ko.observableArray([]);

    /* Functions */
    self.addNewCouse = function (course) {
        // add logic to detect where it should go and prompt user if correct
        self.unassignedCourses.push(course);
    }
}