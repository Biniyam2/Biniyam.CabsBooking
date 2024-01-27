var responseData = "";
var triggerSource;
var triggerSourceInnerHtml;

function loadPartial(route, data, containerID, onComplete) {
    responseData = "";
    routeKey = "";
    route = baseUrl + route;
    $("#spinner").show();
    $(".tt").tooltip('dispose');
    var url = route;
    $.get(url, data)
        .done(function (response) {
            clearTriggerSource();
            if (containerID !== "" && !isJsonData(response)) {
                $("#" + containerID).html(response).fadeIn();
            }
            $("#spinner").hide();
            if (isJsonData(response)) {
                responseData = JSON.stringify(response);
                routeKey = response.key;
            }
            if (onComplete) {
                if (onComplete.runMethod) {
                    if (onComplete.params) {
                        onComplete.runMethod.apply(this, onComplete.params);
                    }
                    else {
                        onComplete.runMethod.apply(this);
                    }
                }
            }
            $(".tt").tooltip();
        })
        .fail(function (response) {
            clearTriggerSource();
            $("#spinner").hide();
            console.log(response);
            alert("Error " + response.statusText);
        });
}

function loadPartialBtn(button, route, data, containerID, onComplete) {
    setTriggerSource(button);
    loadPartial(route, data, containerID, onComplete)
}

function submitPartial(route, data, containerID, onComplete) {
    responseData = "";
    routeKey = "";
    route = baseUrl + route;
    $("#spinner").show();
    $(".tt").tooltip('dispose');
    var url = route;
    setAntiForgeryToken(data);
    $.post(url, data)
        .done(function (response, statusText, xhr) {
            if (containerID !== "" && !isJsonData(response)) {
                $("#" + containerID).html(response);
            }
            $("#spinner").hide();
            clearTriggerSource();

            if (isJsonData(response)) {
                responseData = JSON.stringify(response);
                routeKey = response.key;
            }
            if (xhr.getResponseHeader("HasError") !== "1") {
                if (onComplete) {
                    if (onComplete.runMethod) {
                        if (onComplete.params) {
                            onComplete.runMethod.apply(this, onComplete.params);
                        }
                        else {
                            onComplete.runMethod.apply(this);
                        }
                    }
                }
            }
            $(".tt").tooltip();
        })
        .fail(function (response) {
            $("#spinner").hide();
            clearTriggerSource();
            console.log(response);
            alert("Error " + response.statusText);
        });

}


function submitFormData(route, data, containerID, onComplete) {

    responseData = "";
    routeKey = "";
    route = baseUrl + route;
    $("#spinner").show();
    $(".tt").tooltip('dispose');
    var url = route;

    $.ajax({
        type: "POST",
        enctype: 'multipart/form-data',
        url: url,
        data: data,
        processData: false,
        contentType: false,
        cache: false,
        timeout: 800000,
        success: function (response, statusText, xhr) {
            clearTriggerSource();
            if (containerID !== "" && !isJsonData(response)) {
                $("#" + containerID).html(response).fadeIn();
            }
            $("#spinner").hide();
            if (isJsonData(response)) {
                responseData = JSON.stringify(response);
                routeKey = response.key;
            }
            if (xhr.getResponseHeader("HasError") !== "1") {
                if (onComplete) {
                    if (onComplete.runMethod) {
                        if (onComplete.params) {
                            onComplete.runMethod.apply(this, onComplete.params);
                        }
                        else {
                            onComplete.runMethod.apply(this);
                        }
                    }
                }
            }
            $(".tt").tooltip();
        },
        error: function (response, e) {

            $("#spinner").hide();
            clearTriggerSource();
            console.log(response);
            alert("Error " + response.statusText);

        }
    });
}

function setTriggerSource(button) {
    triggerSource = button;
    triggerSourceInnerHtml = $(button).html();
    $(button).prop('disabled', true);
    $(button).html("<span class='spinner-border spinner-border-sm'></span>");
}
function clearTriggerSource() {
    if (triggerSource) {
        $(triggerSource).prop('disabled', false);
        $(triggerSource).html(triggerSourceInnerHtml);
        triggerSource = null;
        triggerSourceHtml = null;
    }
}
function isJsonData(response) {
    return typeof response === "object";

}
function deletePartial(route, data, containerID, onComplete) {
    if (!confirm("Are you sure you want to delete this record?")) return false;
    loadPartial(route, data, containerID, onComplete);
}

function savePartial(button, onComplete) {
    var formProperties = getFormProperties(button);
    var formData = getFormData(button);
    var route = "/" + formProperties.controller + "/" + formProperties.saveAction;
    setTriggerSource(button);
    submitFormData(route, formData, formProperties.containerId, onComplete);
    uploadFileList = [];
}

function getFormData(button) {
    var formId = $(button).closest("form").attr("id");
    var form = document.getElementById(formId);
    var dto = new FormData(form);

    if (uploadFileList.length > 0) {
        for (var i = 0; i < uploadFileList.length; i++) {
            dto.append("files", uploadFileList[i]);
        }
    }
    return dto;
}
function editPartial(button) {
    var formProperties = getFormProperties(button);
    var route = "/" + formProperties.controller + "/" + formProperties.editAction;
    loadPartial(route, { "id": formProperties.recordId }, formProperties.containerId);
}

function updateProperty(field, callbackFunction) {
    var formProperties = getFormProperties(field);
    var fieldNameArray = field.name.split(".");
    var fieldName = fieldNameArray[fieldNameArray.length - 1];
    var route = "/" + formProperties.controller + "/" + formProperties.saveAction;
    var data = { "id": formProperties.recordId };
    data.property = fieldName;
    if ($(field).is(':checkbox')) {
        data.value = $(field).prop('checked');
    }
    else {
        data.value = $(field).val();
    }

    var methodToCall = callbackFunction ? callbackFunction : updatePropertyCallBack;
    var runMethod = { 'runMethod': methodToCall };
    submitPartial(route, data, "", runMethod);
}

function updatePropertyCallBack() {
    if (responseData !== "") {
        var response = JSON.parse(responseData);
        if (!response.success) {
            alert("Operation failed. Data did not update");
        }
    }
}
function getFormProperties(button) {
    var formProperties = {};
    var form = $(button).closest("form");
    formProperties.controller = $(form).data("controller");
    formProperties.saveAction = $(form).data("save-action") || "Save";
    formProperties.editAction = $(form).data("edit-action") || "Edit";
    formProperties.newAction = $(form).data("new-action") || "New";
    formProperties.containerId = $(form).data("container-id") || $(form).attr("id");
    formProperties.recordId = $(form).data("id");
    return formProperties;
}


function loadCascadeValues(parent) {
    var selectedValue = $(parent).val();
    var controller = $(parent).data("controller");
    var action = $(parent).data("action");
    var targetField = $(parent).data("target-field");
    var route = "/" + controller + "/" + action;
    var params = { parentId: selectedValue };
    loadChildOptions(route, targetField, params);
}


function loadChildOptions(route, targetField, params) {
    var url = baseUrl + route;
    $("#" + targetField).empty();
    $.getJSON(url, params, function (data, status) {
        var item = "<option value=''>-Please Select-</option>";
        if (status === "success") {
            $.each(data, function (i, childData) {
                item += '<option value="' + childData.value + '">' + childData.text + '</option>';
            });
        }
        $("#" + targetField).html(item);
    });
}


function searchJagcProfiles() {
    var searchString = $("#ProfileSearchString").val();
    $("#message-alert").hide();
    if (searchString !== "") {
        loadPartial('/Users/JAGCUserList', { searchString: searchString }, 'jagcProfilesSelectorContainer');
    }
}

function addMembership(btn) {
    var profileId = $(btn).data("id");
    loadPartial("/users/add-membership", { personKey: profileId }, "", { runMethod: addMembershipCallBack });
}

function addMembershipCallBack() {
    if (responseData !== "") {
        var response = JSON.parse(responseData);
        if (response.success) {
            location.href = baseUrl + "/users/details/" + response.personKey;
        }
        else {
            $("#message-alert").html(response.message);
            $("#message-alert").show();
        }
    }
}

function handleSectionHide(shouldHide, id) {
    if (shouldHide) {
        $("#" + id).slideUp();
    }
    else {
        $("#" + id).slideDown();
    }
}

function handleSectionCollapse(btn) {
    var target = $(btn).data("target");
    var outerContainer = $(btn).data("outer-container");


    if ($(btn).hasClass("bi-chevron-up")) {
        $(btn).removeClass("bi-chevron-up");
        $(btn).addClass("bi-chevron-right");
        $(btn).closest(".d-flex").find("h6").removeClass("text-purple");
        $(btn).closest(".d-flex").attr("data-user-collapsed", "true");

        $(btn).closest(".card").removeClass("shadow-sm");

        if (typeof outerContainer !== 'undefined')
            $("#" + outerContainer).slideUp();
        else
            $("#" + target).slideUp();
    }
    else {
        $(btn).removeClass("bi-chevron-right");
        $(btn).addClass("bi-chevron-up");
        $(btn).closest(".card").addClass("shadow-sm");
        if (typeof outerContainer !== 'undefined')
            $("#" + outerContainer).slideDown();
        else
            $("#" + target).slideDown();
        $(btn).closest(".d-flex").find("h6").addClass("text-purple");
        //Check if the data is already loaded
        var dataLoaded = $(btn).closest(".d-flex").attr("data-loaded") == "true";
        if (typeof $(btn).data('path') !== 'undefined' && !dataLoaded) {
            if (target != "chronology-section-container") {
                //Always reload chronology
                $(btn).closest(".d-flex").attr("data-loaded", "true");
            }
            
            loadPartial($(btn).data("path"), {}, target);
        }
    }
}

function setAntiForgeryToken(dto) {
    console.log(dto);
    dto.__RequestVerificationToken = $("INPUT[name=__RequestVerificationToken] ").first().val();
}
function openModal(modalId) {
    new bootstrap.Modal(document.getElementById(modalId), { backdrop: 'static' }).show();
}
function hideModal(modalId) {
    bootstrap.Modal.getInstance(document.getElementById(modalId)).hide();
}

function isEmpty(val) {
    return val === null || val === "";
}
function isInViewport(element) {
    const rect = element.getBoundingClientRect();
    if (typeof(rect) != "undefined") {
        return (
            rect.top >= 0 &&
            rect.left >= 0 &&
            rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
            rect.right <= (window.innerWidth || document.documentElement.clientWidth)
        );
    }
    return false;
}
$(document).ready(
    function () {
        $(".tt").tooltip();
    }
);


