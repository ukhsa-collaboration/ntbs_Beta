import Vue from "vue";
import {getHeaders, buildPath} from "../helpers";
import axios from "axios";

const FetchSpecimenPotentialMatch = Vue.extend({
    data() {
        return {
            timeout: null
        }
    },
    methods: {
        update: function (userInput: string) {
            this.clearPotentialMatchHtml();
            this.clearErrorMessage();
            
            clearTimeout(this.timeout);
            this.timeout = setTimeout(() => {
                this.fetchPotentialMatchData(userInput)
            }, 500);
        },
        fetchPotentialMatchData: function (value: string) {
            const requestConfig = {
                url: buildPath("PotentialMatchData"),
                headers: getHeaders(),
                params: {
                    "manualNotificationId": value
                }
            };

            axios.request(requestConfig)
                .then((response: any) => {
                    if (typeof response.data === "object") {
                        this.showErrorMessage(response.data.errorMessage);
                    } else {
                        this.updatePotentialMatchHtml(response.data);
                    }
                });
        },
        showErrorMessage: function (errorMessage: string) {
            this.$refs["formGroup"].classList.add("nhsuk-form-group--error");
            this.$refs["inputField"].classList.add("nhsuk-input--error");
            this.$refs["errorField"].classList.remove("hidden");
            this.$refs["errorField"].textContent = errorMessage;
        },
        clearErrorMessage: function () {
            this.$refs["formGroup"].classList.remove("nhsuk-form-group--error");
            this.$refs["inputField"].classList.remove("nhsuk-input--error");
            this.$refs["errorField"].classList.add("hidden");
            this.$refs["errorField"].textContent = "";
        },
        updatePotentialMatchHtml(divHtml: string) {
            this.$refs["matchDetails"].innerHTML = divHtml;
            console.log('UpdatingPotentialDiv');
            console.log(divHtml);
        },
        clearPotentialMatchHtml() {
            this.$refs["matchDetails"].innerHTML = "";
        }
    }
});

export default FetchSpecimenPotentialMatch;
