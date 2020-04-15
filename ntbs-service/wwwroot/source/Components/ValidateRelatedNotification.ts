import Vue from "vue";
import { getHeaders, getValidationPath } from "../helpers";
import axios from "axios";

const ValidateRelatedNotification = Vue.extend({
    props: ["model", "allowDraft", "allowLegacyNotifications"],
    data: function() {
        return {
            relatedNotification: {},
            warningMessage: "",
            isValid: false,
            hasWarningMessage: false
        }
    },
    mounted: function() {
        const currentId = this.$refs["inputField"].value;
        if (currentId) {
            this.tryGetNotification(currentId);
        }
    },
    methods: {
        validate: function (event: FocusEvent) {
            const inputField = event.target as HTMLInputElement;
            const newValue = inputField.value;

            this.tryGetNotification(newValue);
        },
        tryGetNotification: function (id: string) {
            const requestConfig = {
                url: `/NotificationSummary/${id}`,
                headers: getHeaders(),
                params: {
                    "allowDraft": this.$props.allowDraft,
                    "allowLegacyNotifications": this.$props.allowLegacyNotifications
                }
            };
            axios.request(requestConfig)
                .then((response: any) => {
                    const responseContent = response.data;
                    const hasError = responseContent.hasOwnProperty('validationMessage');
                    if (hasError) {
                        this.$el.classList.add("nhsuk-form-group--error");
                        this.$refs["errorField"].classList.remove("hidden");
                        this.$refs["inputField"].classList.add("nhsuk-input--error");
                        this.$refs["errorField"].textContent = responseContent.validationMessage;
                        this.relatedNotification = {};
                        this.isValid = false;
                        this.warningMessage = "";
                        this.hasWarningMessage = false;
                    } else {
                        // either blank or valid
                        this.$el.classList.remove("nhsuk-form-group--error");
                        this.$refs["errorField"].classList.add("hidden");
                        this.$refs["inputField"].classList.remove("nhsuk-input--error");
                        this.$refs["errorField"].textContent = '';
                        this.isValid = responseContent.hasOwnProperty('relatedNotification');
                        this.hasWarningMessage = responseContent.hasOwnProperty('warningMessage');
                        
                        if (this.hasWarningMessage) {
                            this.warningMessage = responseContent.warningMessage;
                        } else {
                            this.relatedNotification = this.isValid ? responseContent.relatedNotification : {};
                        }

                    }
                })
                .catch((error: any) => {
                    console.log(error.response);
                });
        }
    }
});

export default ValidateRelatedNotification;