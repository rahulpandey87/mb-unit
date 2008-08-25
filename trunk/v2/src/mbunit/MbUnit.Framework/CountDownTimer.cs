// MbUnit Test Framework
// 
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		MbUnit HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux

using System;
namespace MbUnit.Framework {
    using MbUnit.Core.Monitoring;

    /// <summary>
    /// A basic count down timer for use with <see cref="PerfAssert"/> tests. 
    /// Creates an instance of <see cref="TimeMonitor"/>, starts and stops it
    /// </summary>
    public struct CountDownTimer {

        private double maxDuration;
        private TimeMonitor timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountDownTimer"/> and starts it.
        /// </summary>
        /// <param name="maxDuration">The maximum duration for the timer</param>
        public CountDownTimer(double maxDuration) {
            this.maxDuration = maxDuration;
            this.timer = new TimeMonitor();
            timer.Start();
        }

        /// <summary>
        /// Stops the timer and verifies whether or not the duration of its count is less than the maxDuration specified.
        /// </summary>
        public void Stop() {
            timer.Stop();

            Assert.IsTrue(timer.Duration < this.maxDuration,
                         "Timer duration {0}s is longer that maximum duration {1}s",
                         timer.Duration,
                         this.maxDuration
                         );
        }
    }
}
