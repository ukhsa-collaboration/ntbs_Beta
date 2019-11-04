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
                return this.$refs["inner-validate"].$refs["selectField"].value;
            }
            return null;
        },
        handleChange: function (event: FocusEvent) {
            const inputField = event.target as HTMLInputElement;
            const inputValue = inputField.value;
            this.hideOrShowControlBasedOnValue(inputValue);
        },
        hideOrShowControlBasedOnValue: function (value: String) {
            if (this.$props.valueConditionFunction && this.$props.valueConditionFunction(value)) {
                this.$refs["conditional-control"].classList.remove("hidden");
            } else {
                this.$refs["conditional-control"].classList.add("hidden");
            }
        }
    }
});

export default ConditionalSelectWrapper;