import Vue from "vue";

const NotificationInfo = Vue.extend({
    props: ["notificationInfo"],
    template:`
        <div id="notification-info" class="notification-info">
            <p>Name: {{notificationInfo.Name}}</p>
            <p>DOB: {{notificationInfo.Dob}}</p>
            <p>NHS number: {{notificationInfo.NhsNumber}}</p>
            <p>Drug resistance profile: {{notificationInfo.DrugResistance}}</p>
            <p>Sex: {{notificationInfo.Sex}}</p>
            <p>Postcode: {{notificationInfo.Postcode}}</p>
        </div>
    `
});

export {
    NotificationInfo
};