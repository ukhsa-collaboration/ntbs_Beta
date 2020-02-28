import Vue from "vue";
import { getHeaders, getValidationPath } from "../helpers";
import axios from "axios";

const qs = require("qs");

const ValidateRequiredCheckboxes = Vue.extend({
    props: ["property", "shouldvalidatefull"],
    methods: {
        validate: function () {
            const notificationSiteRegex = /NotificationSiteMap\[(.*)\]/;
            const checkboxes: Array<HTMLInputElement> = Array.from(this.$refs["checkboxgroup"].getElementsByClassName("nhsuk-checkboxes__input"));
            const checkboxList = checkboxes
                .filter(x => x.checked)
                .map(x => notificationSiteRegex.exec(x.name)[1]);

            const queryString = qs.stringify({
                "valueList": checkboxList,
                "shouldValidateFull": this.$props.shouldvalidatefull,
            });

            const requestConfig = {
                url: `${getValidationPath(this.$props.property)}?${queryString}`,
                headers: getHeaders(),
            };

            axios.request(requestConfig)
                .then((response: any) => {
                    const errorMessage = response.data;
                    if (errorMessage) {
                        this.$el.classList.add("nhsuk-form-group--error");
                        this.$refs["errorField"].textContent = errorMessage;
                        this.$refs["errorField"].classList.remove("hidden");
                    } else {
                        this.$el.classList.remove("nhsuk-form-group--error");
                        this.$refs["errorField"].textContent = "";
                        this.$refs["errorField"].classList.add("hidden");
                    }
                })
                .catch((error: any) => {
                    console.log(error.response);
                });
        }
    }
});

export default ValidateRequiredCheckboxes;