import Vue from "vue";

const ConditionalSelectWrapper = Vue.extend({
    props: {
        valueConditionFunction: {
            type: Function
        }
    },
    methods: {
        innerValidateMounted: function () {
            const value = this.getValueFromChild();
            this.hideOrShowControlBasedOnValue(value);
        },
        getValueFromChild: function () {
            if (this.$refs["inner-validate"] && this.$refs["inner-validate"].$refs["selectField"]) {
                const selectField = this.$refs["inner-validate"].$refs["selectField"];
                if (selectField.tagName === "DIV") {
                    return selectField.getElementsByTagName("select")[0].value;
                }
                return selectField.value;
            }
            return null;
        },
        handleChange: function (event: FocusEvent) {
            const inputField = event.target as HTMLInputElement;
            const inputValue = inputField.value;
            this.hideOrShowControlBasedOnValue(inputValue);
        },
        hideOrShowControlBasedOnValue: function (value: String) {
            const conditionalElement = this.getRefElement("conditional-control");
            if (this.$props.valueConditionFunction && this.$props.valueConditionFunction(value)) {
                conditionalElement.classList.remove("hidden");
            } else {
                conditionalElement.classList.add("hidden");
            }
        },
        getRefElement: function (refName: string) {
            const ref = this.$refs[refName];
            if (ref instanceof Vue) {
                return ref.$el;
            } else {
                return ref;
            }
        }
    }
});

export default ConditionalSelectWrapper;