import Vue from "vue";
import InactivityLeaveComponent from "./InactivityLeaveComponent";
import { getHeaders, buildPathRelativeToOrigin } from "../helpers";
import axios from "axios"; 

const InactivityChecker = Vue.extend({
    render(): any { return null },
    mounted: function() {
        this.interval = setInterval(this.checkActivity, 60000);
    },
    
    methods: {
        checkLeave: function () {
            this.$modal.confirm(InactivityLeaveComponent, "You have been inactive")
                .then(() => {
                    window.location.href = buildPathRelativeToOrigin("Logout")
                })
                .catch(() => {
                    this.updateActivity();
                    this.$modal.close()
                });
        },
        checkActivity() {
            const requestConfig = {
                url: buildPathRelativeToOrigin("Heartbeat/IsActive"),
                headers: getHeaders(),
            };
            axios.request(requestConfig)
                .then((response: any) => {
                    if (response.data == "True") {
                        this.$modal.close();
                    } else {
                        this.checkLeave();
                    }
                });
        },
        updateActivity() {
            const requestConfig = {
                url: buildPathRelativeToOrigin("Heartbeat/UpdateActivity"),
                headers: getHeaders(),
            };
            axios.request(requestConfig)
                .catch(function (error) {
                    console.log(error);
                });
        }
    }
});

export default InactivityChecker;