import java.net.HttpCookie;
import scala.collection.JavaConverters._

object Config {
    val urlUnderTest = "https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io"
    val lengthOfTestInMinutes = sys.env.getOrElse("LOAD_TEST_DURATION_IN_MINUTES", "1").toInt
    val cookieHeader = sys.env.getOrElse(
        "COOKIE_HEADER",
        "<insert cookie here>"
    )
    val cookieList = cookieHeader.split(";").map(c => HttpCookie.parse(c).asScala.head)
}
