import rospy
import tf.transformations

from std_msgs.msg import String, Empty
from geometry_msgs.msg import PoseStamped

def callback(msg):
    print "Received position message."

    position = msg.pose.position
    quat = msg.pose.orientation
    #rospy.loginfo("Point Position: [ %f, %f, %f ]"%(position.x, position.y, position.z))
    #rospy.loginfo("Quat Orientation: [ %f, %f, %f, %f]"%(quat.x, quat.y, quat.z, quat.w))

    # Also print Roll, Pitch, Yaw
    euler = tf.transformations.euler_from_quaternion([quat.x, quat.y, quat.z, quat.w])
    #rospy.loginfo("Euler Angles: %s"%str(euler))

    posData = str(position.x) + "," + str(position.y) + "," +str(position.z)
    rotData = str(euler)

    print posData
    print rotData.strip("()")

    rospy.sleep(0.01)

def listener():
    rospy.init_node('position_listener', anonymous=True)

    rospy.Subscriber("/firefly1/ground_truth/pose", PoseStamped, callback)

    # spin() simply keeps python from exiting until this node is stopped
    rospy.spin()

if __name__ == '__main__':
    listener()
