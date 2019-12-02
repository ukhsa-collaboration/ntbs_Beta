import Vue from "vue";
import { getHeaders, getValidationPath } from "../helpers";
import axios from "axios";

const ValidateRelatedNotification = Vue.extend({
    props: ["model"],
    data: function() {
        return {
            relatedNotification: {},
            isValid: false
        }
    },
    mounted: function() {
        var currentId = this.$refs["inputField"].value;
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
                url: `${getValidationPath(this.$props.model)}RelatedNotification`,
                headers: getHeaders(),
                params: {
                    "value": id,
                }
            };
            axios.request(requestConfig)
                .then((response: any) => {
                    var responseContent = response.data;
                    var hasError = responseContent.hasOwnProperty('validationMessage');
                    if (hasError) {
                        this.$el.classList.add("nhsuk-form-group--error");
                        this.$refs["errorField"].classList.remove("hidden");
                        this.$refs["inputField"].classList.add("nhsuk-input--error");
                        this.$refs["errorField"].textContent = responseContent.validationMessage;
                        this.relatedNotification = {};
                        this.isValid = false;
                    } else {
                        // either blank or valid
                        this.$el.classList.remove("nhsuk-form-group--error");
                        this.$refs["errorField"].classList.add("hidden");
                        this.$refs["inputField"].classList.remove("nhsuk-input--error");
                        this.$refs["errorField"].textContent = '';
                        this.isValid = responseContent.hasOwnProperty('relatedNotification');;
                        this.relatedNotification = this.isValid ? responseContent.relatedNotification : {};
                    }
                })
                .catch((error: any) => {
                    console.log(error.response);
                });
        }
    }
});

export {
    ValidateRelatedNotification
};