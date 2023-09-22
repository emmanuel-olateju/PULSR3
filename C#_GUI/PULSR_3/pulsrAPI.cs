using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Management;



namespace PULSR_3
{


    public class Motor
    {
        public int angle { get; set; }
        public int target_speed { get; set; }
        public int link_force { get; set; }
        public int fore_threshold { get; set; }

        public Motor()
        {
            angle = 0;
            target_speed = 10;
            link_force = 0;
            fore_threshold = 0;
        }
    }

    public class pulsr
    {
        public int control_data;
        private string motor_data;
        private int force_data;
        private int wanted_load_cell;
        private bool circleMode;

        public Motor upper;
        public Motor lower;

        private int ll;
        private int lu;
        private int le;
        private int e1;
        private int e2;
        private int xi;
        private int yi;
        public int x;
        public int y;
        private int offset_angle;
        //private double ll, lu, le, e1, e2, xi, yi, x, y, offset_angle;

        private SerialPort load_cell_coms;
        private SerialPort encoder_coms;
        private SerialPort pulsr2_coms;

        private string load_port;
        private string enc_port;
        private string motor_port;

        private const int baudrate = 2000000;

        public bool front { get; private set; }
        public bool back { get; private set; }
        public bool left { get; private set; }
        public bool right { get; private set; }

        public pulsr()
        {
            control_data = 0;
            motor_data = "";
            force_data = 0;
            wanted_load_cell = 0;
            circleMode = false;

            upper = new Motor();
            lower = new Motor();

            ll = 0;
            lu = 0;
            le = 0;
            e1 = 0;
            e2 = 0;
            xi = 0;
            yi = 0;
            x = 0;
            y = 0;
            offset_angle = 0;

            // Read the USB serial numbers from a file
            string usbSerialNos;
            using (System.IO.StreamReader usbFile = new System.IO.StreamReader("usb_ser.txt"))
            {
                usbSerialNos = usbFile.ReadToEnd();
                Console.WriteLine(usbSerialNos);
            }
            string[] portNumbers = usbSerialNos.Split();
            load_port = portNumbers[0];
            enc_port = portNumbers[2];
            motor_port = portNumbers[4];
            var ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                using (var serialPort = new SerialPort(port))
                {
                    var serialNumber = GetSerialNumber(serialPort);
                    if (serialNumber == load_port)
                    {
                        load_port = port;
                    }
                    else if (serialNumber == enc_port)
                    {
                        enc_port = port;
                    }
                    else if (serialNumber == motor_port)
                    {
                        motor_port = port;
                    }
                }
            }
            Console.WriteLine($"{load_port} {enc_port} {motor_port}");

            string GetSerialNumber(SerialPort serialPort)
            {
                try
                {
                    serialPort.Open();
                    return serialPort.ReadExisting().Trim();
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    serialPort.Close();
                }
            }
        }

        public void InitializeCommunication()
        {
            load_cell_coms = new SerialPort(load_port, baudrate);
            load_cell_coms.Open();

            encoder_coms = new SerialPort(enc_port, baudrate);
            encoder_coms.Open();

            pulsr2_coms = new SerialPort(motor_port, baudrate);
            pulsr2_coms.Open();

            Console.WriteLine("load_cell_coms: " + load_port);
            Console.WriteLine("encoder_coms: " + enc_port);
            Console.WriteLine("motor_coms: " + motor_port);

            CheckCommunication();
        }

        public void CloseCommunication()
        {
            pulsr2_coms.Close();
            load_cell_coms.Close();
            encoder_coms.Close();
        }

        public bool CheckCommunication()
        {
            if (load_cell_coms.IsOpen)
            {
                Console.WriteLine("load cell board connected");
            }
            else
            {
                Console.WriteLine("load cell board not found");
                return false;
            }

            if (encoder_coms.IsOpen)
            {
                Console.WriteLine("encoder board connected");
            }
            else
            {
                Console.WriteLine("encoder board not found");
                return false;
            }

            if (pulsr2_coms.IsOpen)
            {
                Console.WriteLine("pulsr communication check successful");
                return true;
            }
            else
            {
                Console.WriteLine("pulsr communication check failed, restart system to reinitialize communication");
                return false;
            }
        }

        public void SendCommand()
        {
            if (!pulsr2_coms.IsOpen)
            {
                pulsr2_coms.Open();
                Console.WriteLine("pulsr communication not open, thus can't write command");
            }
            else
            {
                pulsr2_coms.DiscardInBuffer();
                pulsr2_coms.DiscardOutBuffer();
                pulsr2_coms.Write(new byte[] { (byte)control_data }, 0, 1);
                pulsr2_coms.DiscardInBuffer();
                pulsr2_coms.DiscardOutBuffer();
                pulsr2_coms.Write(new byte[] { (byte)Math.Abs(upper.target_speed) }, 0, 1);
                pulsr2_coms.DiscardInBuffer();
                pulsr2_coms.DiscardOutBuffer();
                pulsr2_coms.Write(new byte[] { (byte)Math.Abs(lower.target_speed) }, 0, 1);


                Console.WriteLine();
            }
        }

        public void UpdateLinkForce()
        {
            if (!load_cell_coms.IsOpen)
            {
                load_cell_coms.Open();
                Console.WriteLine("pulsr communication not open, thus can't write command, restart system");
            }
            else
            {
                load_cell_coms.DiscardInBuffer();
                load_cell_coms.DiscardOutBuffer();
                load_cell_coms.Write(new byte[] { (byte)wanted_load_cell }, 0, 1);
                byte[] force_data = new byte[4];
                load_cell_coms.Read(force_data, 0, 4);
            }
        }

        public void UpdateAngles()
        {
            if (!encoder_coms.IsOpen)
            {
                encoder_coms.Open();
                Console.WriteLine("pulsr communication not open, thus can't write command, restart system");
            }
            else
            {
                encoder_coms.DiscardInBuffer();
                encoder_coms.DiscardOutBuffer();
                encoder_coms.Write(new byte[] { 0 }, 0, 1);
                byte[] motor_data = new byte[5];
                encoder_coms.Read(motor_data, 0, 5);
            }
        }

        public void ResetAngles()
        {
            if (!encoder_coms.IsOpen)
            {
                encoder_coms.Open();
                Console.WriteLine("pulsr communication not open, thus can't write command, restart system");
            }
            else
            {
                encoder_coms.Write(new byte[] { 255 }, 0, 1);
                byte[] motor_data = new byte[5];
                encoder_coms.Read(motor_data, 0, 5);
            }
        }

        public void ComputerMode()
        {
            control_data &= 63;
            SendCommand();
        }

        public void CircleMode()
        {
            control_data |= 32;
            ComputerMode();
        }

        public void KeyMode()
        {
            control_data &= 95;
            ComputerMode();
        }

        public void UserMode()
        {
            control_data |= 64;
            SendCommand();
        }

        public int UpdateUpperLoadCell()
        {
            wanted_load_cell = 1;
            UpdateLinkForce();
            byte[] force_data = new byte[4];
            int h = (force_data[1] << 2) | (force_data[2] >> 3);
            int l = (force_data[2] << 5) | (force_data[3]);
            upper.link_force = (h << 8) + l;
            return upper.link_force;
        }

        public int UpdateLowerLoadCell()
        {
            wanted_load_cell = 2;
            UpdateLinkForce();
            byte[] force_data = new byte[4];
            int h = (force_data[1] << 2) | (force_data[2] >> 3);
            int l = (force_data[2] << 5) | (force_data[3]);
            lower.link_force = (h << 8) + l;
            return lower.link_force;
        }

        public void UpdateMotorAngles()
        {
            UpdateAngles();
            upper.angle = (motor_data[1] << (8)) + motor_data[2];
            lower.angle = (motor_data[3] << 8) + motor_data[4];
        }

        public void UpdateSensorData()
        {
            UpdateLowerLoadCell();
            UpdateUpperLoadCell();
            UpdateMotorAngles();
            Console.WriteLine("angles: " + upper.angle + ", " + lower.angle);
            Console.WriteLine("force: " + upper.link_force + ", " + lower.link_force);
        }

        public void SetMotorDirections(bool front, bool back, bool left, bool right)
        {
            upper.target_speed = front ? Math.Abs(upper.target_speed) : -Math.Abs(upper.target_speed);
            lower.target_speed = back ? Math.Abs(lower.target_speed) : -Math.Abs(lower.target_speed);
            this.front = front;
            this.back = back;
            this.left = left;
            this.right = right;
        }

        public void EnableUpperMotor()
        {
            control_data |= 2;
            SendCommand();
        }

        public void DisableUpperMotor()
        {
            upper.target_speed = 0;
            control_data &= 125;
            SendCommand();
        }

        public void EnableLowerMotor()
        {
            control_data |= 8;
            SendCommand();
        }

        public void DisableLowerMotor()
        {
            lower.target_speed = 0;
            control_data &= 119;
            SendCommand();
        }

        public void UpperMotorCW(int en_time, int speed)
        {
            upper.target_speed = Math.Abs(speed);
            control_data |= 2;
            control_data &= 123;
            SendCommand();
            if (en_time != 0)
            {
                Thread.Sleep(en_time);
                DisableUpperMotor();
            }
        }

        public void UpperMotorCCW(int en_time, int speed)
        {
            upper.target_speed = Math.Abs(speed);
            control_data |= 2;
            control_data |= 4;
            SendCommand();
            if (en_time != 0)
            {
                Thread.Sleep(en_time);
                DisableLowerMotor();
            }
        }

        public void LowerMotorCW(int en_time, int speed)
        {
            lower.target_speed = Math.Abs(speed);
            control_data |= 8;
            control_data &= 111;
            SendCommand();
            if (en_time != 0)
            {
                Thread.Sleep(en_time);
                DisableUpperMotor();
            }
        }

        public void LowerMotorCCW(int en_time, int speed)
        {
            lower.target_speed = Math.Abs(speed);
            control_data |= 8;
            control_data |= 16;
            SendCommand();
            if (en_time != 0)
            {
                Thread.Sleep(en_time);
                DisableLowerMotor();
            }
        }

        public void UpdateMotorSpeed(int upper_target, int lower_target)
        {
            if (upper_target == 0)
            {
                DisableUpperMotor();
            }
            else if (upper_target < 0)
            {
                UpperMotorCW(0, upper_target);
            }
            else if (upper_target > 0)
            {
                UpperMotorCCW(0, upper_target);
            }

            if (lower_target == 0)
            {
                DisableLowerMotor();
            }
            else if (lower_target < 0)
            {
                LowerMotorCW(0, lower_target);
            }
            else if (lower_target > 0)
            {
                LowerMotorCCW(0, lower_target);
            }
        }

        public void DefineGeometry(int ll, int lu, int le)
        {
            this.ll = ll;
            this.lu = lu;
            this.le = le;
        }

        public int[] ForwardKinematics(int upper_link_angle, int lower_link_angle)
        {
            int l_u = lower_link_angle - upper_link_angle;
            int u = upper_link_angle;
            int l = lower_link_angle;
            int f_l_u = 180 - l_u;
            int ll = this.ll;
            int lu = this.lu;
            int le = this.le;
            int ln = this.ll + this.le;
            int alpha = (180 - f_l_u) / 2;
            int m = (int)Math.Sqrt((lu * lu) + (ll * ll) - (2 * lu * ll * Math.Cos(f_l_u)));
            int k = (int)Math.Sqrt((m * m) + (le * le) - (2 * m * le * Math.Cos(alpha + f_l_u)));
            int beta = (int)Math.Asin((le * Math.Sin(alpha + f_l_u)) / k);
            int e1 = (int)(k * Math.Cos(beta + alpha + u));
            int e2 = (int)(k * Math.Sin(beta + alpha + u));
            return new int[] { e2, e1 };
        }

        public void SetOrigin(int offset_angle)
        {
            this.offset_angle = offset_angle;
            int[] coordinates = ComputeXY();
            xi = coordinates[1];
            yi = coordinates[0];
            x = coordinates[1] - xi;
            y = coordinates[0] - yi;
        }

        public int[] ComputePulsrCoordinates()
        {
            UpdateMotorAngles();
            ForwardKinematics(upper.angle, lower.angle);
            //return ComputeXY();
            return new int[] { e2, e1 };
        }

        public int[] ComputeXY()
        {
            ComputePulsrCoordinates();
            int y = (int)(-(e2 * Math.Cos(offset_angle)) - (e1 * Math.Sin(offset_angle)));
            int x = (int)((e1 * Math.Cos(offset_angle)) - (e2 * Math.Sin(offset_angle)));
            //y = glass_to_screen_scaler * y;
            //x = glass_to_screen_scaler * x;
            return new int[] { y, x };
        }

        public int[] ReturnXYCoordinate()
        {
            int[] coordinates = ComputeXY();
            x = coordinates[1] - xi;
            y = coordinates[0] - yi;
            return new int[] { y, x };
        }

        /*public static void Main(string[] args)
        {
            pulsr neuro = new pulsr();
            neuro.InitializeCommunication();
            Thread.Sleep(3000);
            Console.WriteLine("communication handshake successful");
            neuro.lower.angle = 109;
            neuro.upper.angle = 0;
            neuro.DefineGeometry(26, 26, 26);
            neuro.SetOrigin(20);
            neuro.upper.target_speed = 0;
            neuro.lower.target_speed = 0;
            Console.WriteLine(neuro.ReturnXYCoordinate());
            while (true)
            {
                neuro.UpdateSensorData();
                Thread.Sleep(250);
            }
        }*/
    }
}
