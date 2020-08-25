using NBitcoin;
using NBitcoin.DataEncoders;
using NBitcoin.Protocol;
using NBitcoin.RPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace NBitcoin.Altcoins
{
	public class Bitcoinvault : NetworkSetBase
	{
		public static Bitcoinvault Instance { get; } = new Bitcoinvault();

		public override string CryptoCode => "BTCV";

		private Bitcoinvault()
		{

		}
		//Format visual studio
		//{({.*?}), (.*?)}
		//Tuple.Create(new byte[]$1, $2)
		static Tuple<byte[], int>[] pnSeed6_main = {
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x59,0xa3,0x8c,0xcc}, 8333),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x95,0x1c,0x9b,0xe5}, 8333),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xa5,0xe3,0x09,0x48}, 8333),
		};
		static Tuple<byte[], int>[] pnSeed6_test = {
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x58,0x44,0x34,0xac}, 8666),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x25,0x78,0xba,0x55}, 8666),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xbc,0x47,0xdf,0xce}, 8666),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xb9,0xc2,0x8e,0x7a}, 8666),
		};        		     		

#pragma warning disable CS0618 // Type or member is obsolete
		public class BitcoinvaultConsensusFactory : ConsensusFactory
		{
			private BitcoinvaultConsensusFactory()
			{
			}

			public static BitcoinvaultConsensusFactory Instance { get; } = new BitcoinvaultConsensusFactory();

			public override BlockHeader CreateBlockHeader()
			{
				return new BitcoinvaultBlockHeader();
			}
			public override Block CreateBlock()
			{
				return new BitcoinvaultBlock(new BitcoinvaultBlockHeader());
			}
		}

		public class BitcoinvaultBlockHeader : BlockHeader
		{
			public override uint256 GetPoWHash()
			{
				//btcv Timetravel Algo implement here
				throw new NotSupportedException("PoW for BitcoinVault BTCV is not supported");
			}
		}

		public class BitcoinvaultBlock : Block
		{
			public BitcoinvaultBlock(BitcoinvaultBlockHeader header) : base(header)
			{

			}
			public override ConsensusFactory GetConsensusFactory()
			{
				return BitcoinvaultConsensusFactory.Instance;
			}
		}

#pragma warning restore CS0618 // Type or member is obsolete

		protected override void PostInit()
		{
			RegisterDefaultCookiePath("Bitcoinvault");
		}

		protected override NetworkBuilder CreateMainnet()
		{
			NetworkBuilder builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210000, 
				MajorityEnforceBlockUpgrade = 750,
				MajorityRejectBlockOutdated = 950,
				MajorityWindow = 1000,
				//BIP34Hash = new uint256("0x000000000000024b89b42a942fe0d9fea3bb44ab7bd1b19115dd6a759c0808b8"),
				BIP34Hash = new uint256(),
				PowLimit = new Target(new uint256("00000000ffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = TimeSpan.FromSeconds(14 * 24 * 60 * 60),
				PowTargetSpacing = TimeSpan.FromSeconds(10 * 60),
				PowAllowMinDifficultyBlocks = false,
				PowNoRetargeting = false,
				RuleChangeActivationThreshold = 1916,
				MinerConfirmationWindow = 2016,
				CoinbaseMaturity = 100,
				MinimumChainWork = new uint256("0x0000000000000000000000000000000000000000000000000000000000000001"),
				ConsensusFactory = BitcoinvaultConsensusFactory.Instance
				//SupportSegwit = false
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 78 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 60 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 128 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x88, 0xB2, 0x1E })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x88, 0xAD, 0xE4 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("royale"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("royale"))
			.SetMagic(0xadd8bd55) //defined in inverted direction
			.SetPort(8333) 
			.SetRPCPort(8332)
			.SetName("btcv-main")
			.AddAlias("btcv-mainnet")
			.AddAlias("bitcoinvault-mainnet")
			.AddAlias("bitcoinvault-main")
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("seed.bitcoinvault.global", "seed.bitcoinvault.global")
			})
			.AddSeeds(ToSeed(pnSeed6_main)) 
			.SetGenesis("010000000000000000000000000000000000000000000000000000000000000000000000015ad53234c76bb9c0fa67c1591c0ffb8ee32608513ddcd3026adb0b03cc928b40999b5dffff001d8fab010d0101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4d04ffff001d0104455468652054696d65732030332f4a616e2f32303039204368616e63656c6c6f72206f6e206272696e6b206f66207365636f6e64206261696c6f757420666f722062616e6b73ffffffff0100cf1413040000004104678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5fac00000000");
			return builder;
		}

   		protected override NetworkBuilder CreateTestnet()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210000,
				MajorityEnforceBlockUpgrade = 750,
				MajorityRejectBlockOutdated = 950,
				MajorityWindow = 1000,
				//BIP34Hash = new uint256("604148281e5c4b7f2487e5d03cd60d8e6f69411d613f6448034508cea52e9574"),
				BIP34Hash = new uint256(),
				PowLimit = new Target(new uint256("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = TimeSpan.FromSeconds(14 * 24 * 60 * 60),
				PowTargetSpacing = TimeSpan.FromSeconds(10 * 60),
				PowAllowMinDifficultyBlocks = false,
				PowNoRetargeting = false,
				RuleChangeActivationThreshold = 1916,
				MinerConfirmationWindow = 2016,
				CoinbaseMaturity = 100,
				ConsensusFactory = BitcoinvaultConsensusFactory.Instance
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 111 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 196 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x35, 0x87, 0xCF })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x35, 0x83, 0x94 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("troyale"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("troyale"))
			.SetMagic(0xF1C8D2FD) //defined in inverted direction, 0xFDD2C8F1
			.SetPort(8666)
			.SetRPCPort(50332)
			.SetMaxP2PVersion(80000)
			.SetName("btcv-test")
			.AddAlias("btcv-testnet")
			.AddAlias("bitcoinvault-test")
			.AddAlias("bitcoinvault-testnet")
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("testnet.bitcoinvault.global", "testnet.bitcoinvault.global")
			})
			.AddSeeds(ToSeed(pnSeed6_test))
			.SetGenesis("010000000000000000000000000000000000000000000000000000000000000000000000ff00e3481f61b255420602f7af626924221a41224b0d645bd2f082f82c8bc50a5746ff58f0ff0f1e98611a000101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4004ffff001d010438506f77657264652062792042697473656e642d4575726f7065636f696e2d4469616d6f6e642d4d41432d42332032332f4170722f32303137ffffffff01807c814a00000000434104678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5fac00000000");
			return builder;
		}

		protected override NetworkBuilder CreateRegtest()
		{
			var builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				SubsidyHalvingInterval = 210000,
				MajorityEnforceBlockUpgrade = 750,
				MajorityRejectBlockOutdated = 950,
				MajorityWindow = 1000,
				//BIP34Hash = new uint256("604148281e5c4b7f2487e5d03cd60d8e6f69411d613f6448034508cea52e9574"),
				BIP34Hash = new uint256(),
				PowLimit = new Target(new uint256("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = TimeSpan.FromSeconds(14 * 24 * 60 * 60),
				PowTargetSpacing = TimeSpan.FromSeconds(10 * 60),
				PowAllowMinDifficultyBlocks = false,
				PowNoRetargeting = false,
				RuleChangeActivationThreshold = 1916,
				MinerConfirmationWindow = 2016,
				CoinbaseMaturity = 100,
				ConsensusFactory = BitcoinvaultConsensusFactory.Instance
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 111 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 196 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 239 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x35, 0x87, 0xCF })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x35, 0x83, 0x94 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("troyale")) 
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("troyale")) 
			.SetMagic(0xDAB5BFFA) //defined in inverted direction, 0xFABFB5DA
			.SetPort(19444)
			.SetRPCPort(19332)
			.SetMaxP2PVersion(80000)
			.SetName("btcv-reg")
			.AddAlias("btcv-regtest")
			.AddAlias("bitcoinvault-reg")
			.AddAlias("bitcoinvault-regtest")
			.SetGenesis("010000000000000000000000000000000000000000000000000000000000000000000000c787795041016d5ee652e55e3a6aeff6c8019cf0c525887337e0b4206552691613f7fc58f0ff0f1ea12400000101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4004ffff001d010438506f77657264652062792042697473656e642d4575726f7065636f696e2d4469616d6f6e642d4d41432d42332032332f4170722f32303137ffffffff010000000000000000434104678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5fac00000000");
			return builder;
		}
	}
}
