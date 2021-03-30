import Vue from "vue";
import axios, {Method} from "axios";
import {buildPath, getHeaders} from "../helpers";

const PrintButton = Vue.extend({
    mounted:  function () {
        this.$el.classList.remove("hidden");
    },
    methods: {
        print: function () {
            window.print();
            const requestConfig = {
                method: "post" as Method,
                url: buildPath('AuditPrint'),
                headers: getHeaders()
            };
            axios.request(requestConfig)
                .catch((error: any) => {
                    console.log(error.response);
                });
        }
    }
});

export default PrintButton;