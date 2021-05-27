object Config {
    val urlUnderTest = "https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io"
    val lengthOfTestInMinutes = sys.env.getOrElse("LOAD_TEST_DURATION_IN_MINUTES", "5").toInt
}
