import Vue from "vue";
import InactivityLeaveComponent from "./InactivityLeaveComponent";
import { buildPathRelativeToOrigin } from "../helpers";
import axios from "axios"; 

const OneMinuteInMilliseconds = 60000;

const InactivityChecker = Vue.extend({
    render(): any { return null },
    mounted: function() {
        this.interval = setInterval(this.checkActivity, OneMinuteInMilliseconds);
    },
    
    methods: {
        checkActivity() {
            const requestConfig = {
                url: buildPathRelativeToOrigin("Heartbeat/IsActive"),
                headers: { 
                    'Cache-Control' : 'no-cache, no-store', 
                    'Pragma' : 'no-cache'
                }
            };
            axios.request(requestConfig)
                .then((response: any) => {
                    if (response.data.isActive) {
                        this.$modal.close();
                    } else {
                        this.promptLogoutModal();
                    }
                });
        },
        promptLogoutModal: function () {
            this.$modal.confirm(InactivityLeaveComponent, "You have been inactive")
                .then(() => {
                    window.location.href = buildPathRelativeToOrigin("Logout")
                })
                .catch(() => {
                    this.updateActivity();
                    this.$modal.close()
                });
        },
        updateActivity() {
            const requestConfig = {
                url: buildPathRelativeToOrigin("Heartbeat/UpdateActivity")
            };
            axios.request(requestConfig)
                .catch(function (error) {
                    console.log(error);
                });
        }
    }
});

export default InactivityChecker;