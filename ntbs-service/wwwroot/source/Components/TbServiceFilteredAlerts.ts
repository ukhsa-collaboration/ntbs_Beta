import Vue from "vue";
import { getHeaders } from "../helpers";
import axios from "axios";

const TbServiceFilteredAlerts = Vue.extend({
    methods: {
        updateTable: function () {
        //     const selectedTbService = this.getTbService();
            document.querySelectorAll("tr")

        //     values.forEach(item => optionInnerHtml += `<option value="${item.value}">${item.text}</option>`);
        //     selectElement.innerHTML = optionInnerHtml;
        //     this.setSelectToValueOrDefault(selectElement, currentSelectedValue);
        // },
        // getTbService: function () {
        //     return this.$refs["tbService"].$refs["selectField"].value;
        }
    }
});

export default TbServiceFilteredAlerts;