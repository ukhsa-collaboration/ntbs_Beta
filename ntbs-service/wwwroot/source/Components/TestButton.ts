import Vue from 'vue';

const TestButton = Vue.extend({
  data: () => ({
    text: 'Notify'
  }),
  mounted: function () {
    this.$el.innerText = this.text;
  },
  methods: {
    onBlur: function () {
      this.$el.innerText = "Bye!"
    }
  }
});

export {
  TestButton
};