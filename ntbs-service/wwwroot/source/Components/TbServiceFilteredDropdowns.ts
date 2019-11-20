import Vue from "vue";
import { getHeaders } from "../helpers";
import axios from "axios";

const TbServiceFilteredDropdowns = Vue.extend({
    methods: {
        filteringValidateMounted: function () {
            this.fetchFilteredLists(this.getTbService());
        },
        filteringValidateChanged: function () {
            this.fetchFilteredLists(this.getTbService());
        },
        getTbService: function () {
            return this.$refs["tbService"].$refs["selectField"].value;
        },
        fetchFilteredLists: function (tbServiceCode: string) {
            const requestConfig = {
                url: `${window.location.pathname}/GetFilteredListsByTbService`,
                headers: getHeaders(),
                params: {
                    "tbServiceCode": tbServiceCode
                }
            }

            axios.request(requestConfig)
                .then((response: any) => {
                    for (let key in response.data) {
                        if (response.data.hasOwnProperty(key)) {
                            this.updateSelectList(key, response.data[key]);
                        }
                    }
                });
        },
        updateSelectList: function (refName: string, values: { value: string, text: string }[]) {
            const selectElement = this.$refs[refName].$refs["selectField"] as HTMLSelectElement;
            const currentSelectedValue = selectElement.value;
            let optionInnerHtml: string;
            if (refName === "caseManagers") {
                optionInnerHtml = "<option value=\"\">Select case manager</option>";
            } else if (refName === "hospitals") {
                optionInnerHtml = "<option value=\"\">Select Hospital</option>";
            }

            values.forEach(item => optionInnerHtml += `<option value="${item.value}">${item.text}</option>`);
            selectElement.innerHTML = optionInnerHtml;
            this.setSelectToValueOrDefault(selectElement, currentSelectedValue);
        },
        setSelectToValueOrDefault: function (selectElement: HTMLSelectElement, targetValue: string) {
            for (let i = 0; i < selectElement.options.length; i++) {
                if (selectElement.options[i].value === targetValue) {
                    selectElement.value = targetValue;
                    return;
                }
            }

            selectElement.value = "";
        }
    }
});

export default TbServiceFilteredDropdowns;