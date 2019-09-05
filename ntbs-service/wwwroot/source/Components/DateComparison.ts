import Vue from 'vue';

// Component to validate date order. Compares the members of the "dates" array, in ascending index order.
// Bind dates using v-model, and have bound component emit a datechanged event when date changes.
const DateComparison = Vue.extend({
    data: function() {
      return {
        dates: [],
        dateComparisonError: ''
      }
    },
    methods: {
        datechanged: function (rank: any) {
            var numberOfDates = this.dates.length;
            var currentIndex = parseInt(rank, 10);

            var lowerDateIndex = currentIndex - 1;
            while (lowerDateIndex >= 0 && this.dates[lowerDateIndex])
            {
                if (this.dates[lowerDateIndex] > this.dates[currentIndex])
                {
                    this.dateComparisonError = `Warning: ${this.$refs[`date${currentIndex}`].name} is earlier than ${this.$refs[`date${lowerDateIndex}`].name}`;
                    return;
                }
                lowerDateIndex--;
            }

            var higherDateIndex = currentIndex + 1;
            while (higherDateIndex < numberOfDates && this.dates[higherDateIndex])
            {
                if (this.dates[higherDateIndex] < this.dates[currentIndex])
                {
                    this.dateComparisonError = `Warning: ${this.$refs[`date${currentIndex}`].name} is later than ${this.$refs[`date${higherDateIndex}`].name}`;
                    return;
                }
                higherDateIndex++;
            }
            this.dateComparisonError = '';
        }
    },
});

export {
    DateComparison
};