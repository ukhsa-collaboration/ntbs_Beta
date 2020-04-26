import Vue from "vue";

const NotificationWarning = Vue.extend({
    props: ["warningMessage"],
    template: `
        <div id="notification-info" class="notification-info">
            <p>{{warningMessage}}</p>
        </div>
    `
});

export default NotificationWarning;
